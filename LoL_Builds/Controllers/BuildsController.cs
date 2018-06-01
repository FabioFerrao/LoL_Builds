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
    public class BuildsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Builds
        public ActionResult Index()
        {
            var builds = db.Builds.Include(b => b.Champion);
            return View(builds.ToList());
        }

        // GET: Builds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Builds builds = db.Builds.Find(id);
            if (builds == null)
            {
                return HttpNotFound();
            }
            return View(builds);
        }

        // GET: Builds/Create
        public ActionResult Create()
        {
            ViewBag.ChampionsFK = new SelectList(db.Champions, "ID", "Nome");
            ViewBag.listaItems = db.Items.ToList();
            return View();
        }

        // POST: Builds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,ChampionsFK")] Builds build, FormCollection form)
        {

            string aux = form["checkRole"];
            ICollection<Items> lista = new List<Items> { };
            var x = aux.Split(',');
            foreach (string ids in x)
            {
                Items item = db.Items.Find(Int32.Parse(ids));
                lista.Add(item);
                item.Builds.Add(build);
            }
            build.Items = lista;
            if (ModelState.IsValid)
            {
                db.Builds.Add(build);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChampionsFK = new SelectList(db.Champions, "ID", "Nome", build.ChampionsFK);
            return View(build);
        }

        // GET: Builds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Builds builds = db.Builds.Find(id);
            if (builds == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChampionsFK = new SelectList(db.Champions, "ID", "Nome", builds.ChampionsFK);
            return View(builds);
        }

        // POST: Builds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,ChampionsFK")] Builds builds)
        {
            if (ModelState.IsValid)
            {
                db.Entry(builds).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChampionsFK = new SelectList(db.Champions, "ID", "Nome", builds.ChampionsFK);
            return View(builds);
        }

        // GET: Builds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Builds builds = db.Builds.Find(id);
            if (builds == null)
            {
                return HttpNotFound();
            }
            return View(builds);
        }

        // POST: Builds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Builds builds = db.Builds.Find(id);
            builds.Items = new List<Items>{ };
            db.Builds.Remove(builds);
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
