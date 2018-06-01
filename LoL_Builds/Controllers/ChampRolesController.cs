using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoL_Builds.Models;

namespace LoL_Builds.Controllers
{
    public class ChampRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChampRoles
        public ActionResult Index()
        {
            return View(db.ChampRoles.ToList());
        }

        // GET: ChampRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChampRoles champRoles = db.ChampRoles.Find(id);
            if (champRoles == null)
            {
                return HttpNotFound();
            }
            return View(champRoles);
        }

        [Authorize]
        // GET: ChampRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChampRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Role")] ChampRoles champRoles)
        {
            if (ModelState.IsValid)
            {
                db.ChampRoles.Add(champRoles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(champRoles);
        }

        // GET: ChampRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChampRoles champRoles = db.ChampRoles.Find(id);
            if (champRoles == null)
            {
                return HttpNotFound();
            }
            return View(champRoles);
        }

        // POST: ChampRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Role")] ChampRoles champRoles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(champRoles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(champRoles);
        }

        // GET: ChampRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChampRoles champRoles = db.ChampRoles.Find(id);
            if (champRoles == null)
            {
                return HttpNotFound();
            }
            return View(champRoles);
        }

        // POST: ChampRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChampRoles champRoles = db.ChampRoles.Find(id);
            db.ChampRoles.Remove(champRoles);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
