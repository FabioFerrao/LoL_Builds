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
            builds = builds.OrderBy(b => b.Champion.Nome);
            return View(builds.ToList());
        }

        // GET: Builds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Builds");
            }
            Builds build = db.Builds.Find(id);
            if (build == null)
            {
                return RedirectToAction("Index", "Builds");
            }
            return View(build);
        }


        /// <summary>
        ///     POST: Builds/Details/5
        ///     Metodo para o utilizador comentar a build
        /// </summary>
        /// <param name="id">
        /// id relativo à build que vai ser comentada
        /// </param>
        /// <param name="comentario">
        /// o texto que o utilizador inseriu para comentar a build
        /// </param>
        /// <returns>
        ///     Volta para os detalhes da build depois de comentar 
        /// </returns>
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

        // 
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// POST: Builds/Create 
        /// Metodo que cria builds
        /// </summary>
        /// <param name="build">
        /// recebe a build por parametro, mais propriamente o id, nome,championsFK e UtilizadorFK,
        /// com esses dados vai criar uma build adicionando mais alguns parametros
        /// </param>
        /// <param name="form">
        /// o parametro form é passado para ir buscar os itens onde o check foi selecionado
        /// </param>
        /// <returns>
        ///     depois da criacao da build, retorna para o index das builds
        /// </returns>
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
                return RedirectToAction("Index", "Builds");
            }
            Builds build = db.Builds.Find(id);
            if (build == null)
            {
                return RedirectToAction("Index", "Builds");
            }
            ViewBag.listaItems = db.Items.ToList();
            ViewBag.ChampionsFK = new SelectList(db.Champions, "ID", "Nome", build.ChampionsFK);
            return View(build);
        }

        // POST: Builds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// POST: Builds/Edit/5
        /// Metodo que recebe o formulario de uma build e altera os valores 
        /// </summary>
        /// <param name="formBuild">
        /// recebe por parametro o formulario da build, para poder edita-lo 
        /// </param>
        /// <returns>
        ///     Retorna para os detalhes da build caso a edicao seja aceite 
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection formBuild)
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
            if (id == null)
            {
                return RedirectToAction("Index", "Builds");
            }
            Builds build = db.Builds.Find(id);
            if (build == null)
            {
                return RedirectToAction("Index", "Builds");
            }
            if (User.Identity.Name == build.Utilizador.UserName || User.IsInRole("Administrador") || User.IsInRole("Moderador"))
            {
                return View(build);
            }
            else
            {
                return RedirectToAction("Index");

            }
        }


        /// <summary>
        ///     POST: Builds/Delete/5
        ///     Metodo que elimina uma build
        /// </summary>
        /// <param name="id">
        ///        id da build a eliminar
        /// </param>
        /// <returns>
        ///     retorna para o index das builds
        /// </returns>
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
