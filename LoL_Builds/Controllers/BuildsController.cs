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

        // POST: Builds/Details/5   
        [HttpPost]
        public ActionResult Details(int id, string comentario)
        {
            Builds builds = db.Builds.Find(id);

            Comentarios comment = new Comentarios();
            comment.BuildID = id;
            comment.Texto = comentario;
            comment.TimeStamp = DateTime.Now;

            var email = User.Identity.GetUserName();
            var utilizador = db.Utilizadores.Where(u => u.UserName.Equals(email)).FirstOrDefault();
            comment.UserID = utilizador.ID;

            if (comentario == "")
            {
                return RedirectToAction("Details");
            }

            if (ModelState.IsValid)
            {
                db.Comentarios.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details");
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
            if (form["checkItem"] != null)
            {
                string aux = form["checkItem"];
                ICollection<Items> lista = new List<Items> { };
                var x = aux.Split(',');
                foreach (string ids in x)
                {
                    Items item = db.Items.Find(Int32.Parse(ids));
                    lista.Add(item);
                    item.Builds.Add(build);
                }
                build.Items = lista;
            }
            else
            {
                build.Items = new List<Items>();
            }

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
            else
            {
                foreach (Items item in db.Items.ToList())
                {
                    if (item.Builds.Contains(builds))
                    {
                        item.Builds.Remove(builds);
                    }
                }

                builds.Items = new List<Items>();

            }

            if (ModelState.IsValid)
            {
                db.Entry(builds).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Details", "Builds", new { id = builds.ID });
            }
            ViewBag.ChampionsFK = new SelectList(db.Champions, "ID", "Nome", builds.ChampionsFK);
            return View(builds);
        }

        // GET: Builds/Delete/5
        public ActionResult Delete(int? id)
        {
            Builds build = db.Builds.Find(id);
            if (User.Identity.Name == build.Utilizador.UserName || User.IsInRole("Administrador") || User.IsInRole("Moderador"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (build == null)
                {
                    return HttpNotFound();
                }
                return View(build);
            }
            else
            {
                return RedirectToAction("Index");

            }
        }

        // POST: Builds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Builds build = db.Builds.Find(id);

            while (build.Items.Count() != 0)
            {
                build.Items.First().Builds.Remove(build);
                build.Items.Remove(build.Items.First());
            }
            build.Items = new List<Items> { };

            while (build.Comentarios.Count() != 0)
            {
                db.Comentarios.Remove(build.Comentarios.First());
            }

            build.Comentarios = new List<Comentarios> { };

            db.Builds.Remove(build);
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
