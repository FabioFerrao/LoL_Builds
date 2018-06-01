using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoL_Builds.Models;

namespace LoL_Builds.Controllers
{
    public class ChampionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Champions
        public ActionResult Index()
        {
            return View(db.Champions.ToList().OrderBy(c => c.Nome));
        }

        // GET: Champions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 
            Champions champions = db.Champions.Find(id);
            if (champions == null)
            {
                return HttpNotFound();
            }
            return View(champions);
        }

        // GET: Champions/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create( )
        {
            ViewBag.listaRoles = db.ChampRoles.ToList();
            return View();
        }

        // POST: Champions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,Descricao,Imagem")] Champions champion, HttpPostedFileBase uploadImage, FormCollection form)
        {

            string nomeImage = champion.Nome.Replace(" ", "") + DateTime.Now.ToString("_yyyyMMdd_hhmmss") + ".png";
            string caminhoParaImagem = Path.Combine(Server.MapPath("~/Imagens/Champions/"), nomeImage);
            if (uploadImage != null)
            {
                champion.Imagem = nomeImage;
            }

            string aux = form["checkRole"];
            ICollection<ChampRoles> lista = new List<ChampRoles> { };
            var x = aux.Split(',');
            foreach (string ids in x)
            {
                ChampRoles champRole = db.ChampRoles.Find(Int32.Parse(ids));
                lista.Add(champRole);
                champRole.Champions.Add(champion);
            }
            champion.ChampRoles = lista;
            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadImage != null)
                    {
                        uploadImage.SaveAs(caminhoParaImagem);
                        db.Champions.Add(champion);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Ocorreu o seguinte erro: " + e.Message);
                }
            }

            return View(champion);
        }

        // GET: Champions/Edit/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Champions champions = db.Champions.Find(id);
            if (champions == null)
            {
                return HttpNotFound();
            }
            return View(champions);
        }

        // POST: Champions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Descricao,Imagem")] Champions champion, HttpPostedFileBase uploadImage)
        {
            string nomeImage = champion.Nome.Replace(" ", "") + DateTime.Now.ToString("_yyyyMMdd_hhmmss") + ".png";
            string nomeVelho = champion.Imagem;
            string caminhoParaImagem = Path.Combine(Server.MapPath("~/Imagens/Champions/"), nomeImage);
            if (uploadImage != null)
            {
                champion.Imagem = nomeImage;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(champion).State = EntityState.Modified;
                    db.SaveChanges();
                    if (uploadImage != null)
                    {
                        uploadImage.SaveAs(caminhoParaImagem);
                        System.IO.File.Delete(Path.Combine(Server.MapPath("~/Imagens/Champions/"), nomeVelho));
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Ocorreu o seguinte erro: " + e.Message);
                }
            }
            return View(champion);
        }

        // GET: Champions/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Champions champions = db.Champions.Find(id);
            if (champions == null)
            {
                return HttpNotFound();
            }
            return View(champions);
        }

        // POST: Champions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {


            Champions champion = db.Champions.Find(id);
            string nomeImagem = champion.Imagem;
            try
            {
                
                db.Champions.Remove(champion);
                db.SaveChanges();
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Imagens/Champions/"), nomeImagem));
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Ocorreu o seguinte erro: " + e.Message);
            }
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
