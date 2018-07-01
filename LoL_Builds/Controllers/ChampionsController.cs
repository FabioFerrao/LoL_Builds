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
        /// <summary>
        ///     POST: Champions/Create
        ///     Metodo que cria champions
        /// </summary>
        /// <param name="champion">
        /// recebe o champion por parametro, mais propriamente o id, o nome, a descricao e imagem,
        /// com esses dados vai criar um champion adicionando mais alguns parametros
        /// </param>
        /// <param name="uploadImage">
        ///     recebe um ficheiro que o utilizador deu upload
        /// </param>
        /// <param name="form">
        /// o parametro form é passado para ir buscar os roles onde o check foi selecionado
        /// </param>
        /// <returns>
        /// depois da criacao do champion, retorna para o index dos champions
        /// </returns>
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


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// POST: Champions/Edit/5
        /// Metodo que edita os atributos do champion
        /// </summary>
        /// <param name="uploadImage">
        ///     recebe um ficheiro que o utilizador deu upload
        /// </param>
        /// <param name="formChampion">
        /// o parametro form é passado para ir buscar os roles onde o check foi selecionado
        /// </param>
        /// <returns>
        /// depois da edicao do champion, retorna para o index dos champions
        /// </returns>
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

        /// <summary>
        ///     GET: Champions/Delete/5
        ///     Metodo que retorna para o index dos champions, pois não é permitido eliminar champions
        /// </summary>
        /// <param name="id">
        ///     id do champion para eliminação 
        /// </param>
        /// <returns>
        /// retorna para o index dos champions
        /// </returns>
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            return RedirectToAction("Index", "Champions");
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
