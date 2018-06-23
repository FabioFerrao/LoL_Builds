using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoL_Builds.Models;
using Microsoft.AspNet.Identity;

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
        public ActionResult Create([Bind(Include = "ID,Nome,ChampionsFK, UtilizadorFK")] Builds build, FormCollection form)
        {
            var email = User.Identity.GetUserName();
            var utilizadorArray = db.Utilizadores.Where(u => u.UserName.Equals(email));
            var i = 0;
            foreach (var u in utilizadorArray)
            {
                i = u.ID;
            }
            Session["idUtilizador"] = i;
            build.UtilizadorFK = i;
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
            ViewBag.listaItems = db.Items.ToList();
            ViewBag.ChampionsFK = new SelectList(db.Champions, "ID", "Nome", builds.ChampionsFK);
            return View(builds);
        }

        // POST: Builds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "ID,Nome,ChampionsFK")] Builds builds*/FormCollection formBuild)
        {
            Builds builds = db.Builds.Find(Int32.Parse(formBuild["ID"]));

            builds.Nome = formBuild["Nome"];
            builds.ChampionsFK = Int32.Parse(formBuild["ChampionsFK"]);

            if (formBuild["checkItem"] != null)
            {
                var x = formBuild["checkItem"].Split(',');

                foreach (Items item in db.Items.ToList())
                {
                    if (x.Contains(item.ID.ToString()))
                    {
                        builds.Items.Add(item);
                        if (!item.Builds.Contains(builds))
                        {
                            item.Builds.Add(builds);
                        }
                    }
                    else
                    {
                        if (item.Builds.Contains(builds))
                        {
                            item.Builds.Remove(builds);
                        }
                    }

                }
            }

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
            builds.Items = new List<Items> { };
            //Items[] buildItems = db.Items.Where(model => model.Builds.Contains(builds)).ToArray();
            //foreach (Items item in buildItems)
            //{
            //    db.Items.Remove(item);
            //}
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
