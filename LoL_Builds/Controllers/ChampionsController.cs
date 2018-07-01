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
        public ActionResult Create()
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
            ViewBag.listaRoles = db.ChampRoles.ToList();
            string aux = form["checkRole"];
            //if (aux == null)
            //{
            //    return View(champion);
            //}
            ICollection<ChampRoles> lista = new List<ChampRoles> { };
            if (aux != null)
            {
                var x = aux.Split(',');
                foreach (string ids in x)
                {
                    ChampRoles champRole = db.ChampRoles.Find(Int32.Parse(ids));
                    lista.Add(champRole);
                    champRole.Champions.Add(champion);
                }
                champion.ChampRoles = lista;
            }
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
            ViewBag.listaRoles = db.ChampRoles.ToList();
            return View(champions);
        }

        // POST: Champions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase uploadImage, FormCollection formChampion)
        {
            Champions champion = db.Champions.Find(Int32.Parse(formChampion["ID"]));

            champion.Nome = formChampion["Nome"];
            champion.Descricao = formChampion["Descricao"];
            champion.Imagem = formChampion["Imagem"];

            string nomeImage = champion.Nome.Replace(" ", "") + DateTime.Now.ToString("_yyyyMMdd_hhmmss") + ".png";
            string nomeVelho = champion.Imagem;
            string caminhoParaImagem = Path.Combine(Server.MapPath("~/Imagens/Champions/"), nomeImage);
            if (uploadImage != null)
            {
                champion.Imagem = nomeImage;
            }

            if (formChampion["SelectedRole"] != null)
            {
                var x = formChampion["SelectedRole"].Split(',');

                foreach (ChampRoles item in db.ChampRoles.ToList())
                {
                    if (x.Contains(item.ID.ToString()))
                    {
                        champion.ChampRoles.Add(item);
                        if (!item.Champions.Contains(champion))
                        {
                            item.Champions.Add(champion);
                        }
                    }
                    else
                    {
                        if (item.Champions.Contains(champion))
                        {
                            item.Champions.Remove(champion);
                        }
                    }

                }
            }
            else
            {

                foreach (ChampRoles role in db.ChampRoles.ToList())
                {
                    if (role.Champions.Contains(champion))
                    {
                        role.Champions.Remove(champion);
                    }
                }

                champion.ChampRoles = new List<ChampRoles>();
            }
            if (ModelState.IsValid)
            {
                //selectedRole = selectedRole ?? new string[] { };
                //var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());
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
            ViewBag.listaRoles = db.ChampRoles.ToList();
            return View(champion);
        }

        //GET: Champions/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Champions champions = db.Champions.Find(id);
            //if (champions == null)
            //{
            //    return HttpNotFound();
            //}
            return RedirectToAction("Index", "Champions");
        }

        //POST: Champions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{


        //    Champions champion = db.Champions.Find(id);
        //    string nomeImagem = champion.Imagem;

        //    foreach (ChampRoles champRole in champion.ChampRoles)
        //    {
        //        champRole.Champions.Remove(champion);
        //    }
        //    champion.ChampRoles = new List<ChampRoles>();

        //    foreach (Builds champBuild in champion.Builds)
        //    {
        //        db.Items.RemoveRange(champBuild.Items);
        //        champBuild.Items = new List<Items>();

        //        db.Comentarios.RemoveRange(champBuild.Comentarios);
        //        champBuild.Comentarios = new List<Comentarios>();
        //    }
        //    db.Builds.RemoveRange(champion.Builds);
        //    champion.Builds = new List<Builds>();
        //    db.ChampRoles.Where(model => model.Champions.Contains(champion));


        //    ChampRoles[] championRoles = db.ChampRoles.Where(model => model.Champions.Contains(champion)).ToArray();
        //    Builds[] championBuilds = db.Builds.Where(model => model.ChampionsFK == id).ToArray();

        //    foreach (ChampRoles champRole in championRoles)
        //    {
        //        db.ChampRoles.Remove(champRole);
        //    }



        //    foreach (Builds champBuild in championBuilds)
        //    {

        //        Items[] buildItems = db.Items.Where(model => model.Builds.Contains(champBuild)).ToArray();
        //        Comentarios[] buildComentarios = db.Comentarios.Where(model => model.BuildID == champBuild.ID).ToArray();
        //        foreach (Items item in buildItems)
        //        {
        //            db.Items.Remove(item);
        //        }

        //        foreach (Comentarios comment in buildComentarios)
        //        {
        //            db.Comentarios.Remove(comment);
        //        }
        //        db.Builds.Remove(champBuild);
        //    }
        //    try
        //    {

        //        db.Champions.Remove(champion);
        //        db.SaveChanges();
        //        System.IO.File.Delete(Path.Combine(Server.MapPath("~/Imagens/Champions/"), nomeImagem));
        //        return RedirectToAction("Index");

        //    }
        //    catch (Exception e)
        //    {
        //        ModelState.AddModelError("", "Ocorreu o seguinte erro: " + e.Message);
        //    }
        //    return RedirectToAction("Index");
        //}

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
