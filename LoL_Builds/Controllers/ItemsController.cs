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
    public class ItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Items
        public ActionResult Index()
        {

            return View(db.Items.ToList().OrderBy(i => i.Nome));
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Items");
            }
            Items item = db.Items.Find(id);
            if (item == null)
            {
                return RedirectToAction("Index", "Items");
            }
            return View(item);
        }

        // GET: Items/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }
        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        ///     POST: Items/Create
        ///     Metodo que cria items
        /// </summary>
        /// <param name="item">
        /// recebe o item por parametro, mais propriamente o id, o nome, a descricao e imagem,
        /// com esses dados vai criar um item adicionando mais alguns parametros
        /// </param>
        /// <param name="uploadImage">
        ///     recebe um ficheiro que o utilizador deu upload
        /// </param>
        /// <returns>
        /// depois da criacao do item, retorna para o index dos items
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,Descricao,Imagem")] Items item, HttpPostedFileBase uploadImage)
        {
            string nomeImage = item.Nome.Replace(" ", "") + DateTime.Now.ToString("_yyyyMMdd_hhmmss") + ".png";
            string caminhoParaImagem = Path.Combine(Server.MapPath("~/Imagens/Items/"), nomeImage);
            if (uploadImage != null)
            {
                item.Imagem = nomeImage;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadImage != null)
                    {
                        uploadImage.SaveAs(caminhoParaImagem);
                        db.Items.Add(item);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Ocorreu o seguinte erro: " + e.Message);
                }
            }
            return View(item);
        }

        // GET: Items/Edit/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Items");
            }
            Items item = db.Items.Find(id);
            if (item == null)
            {
                return RedirectToAction("Index", "Items");
            }
            return View(item);
        }

        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// POST: Items/Edit/5
        /// Metodo que edita os atributos do item
        /// </summary>
        /// <param name="item">
        /// recebe o item por parametro, mais propriamente o id, o nome, a descricao e imagem,
        /// com esses dados vai criar um item adicionando mais alguns parametros
        /// </param>
        /// <param name="uploadImage">
        ///     recebe um ficheiro que o utilizador deu upload
        /// </param>
        /// <returns>
        /// depois da edicao do item, retorna para o index dos items
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Descricao,Imagem")] Items item, HttpPostedFileBase uploadImage)
        {
            string nomeImage = item.Nome.Replace(" ", "") + DateTime.Now.ToString("_yyyyMMdd_hhmmss") + ".png";
            string nomeVelho = item.Imagem;
            string caminhoParaImagem = Path.Combine(Server.MapPath("~/Imagens/Items/"), nomeImage);
            if (uploadImage != null)
            {
                item.Imagem = nomeImage;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    if (uploadImage != null)
                    {
                        uploadImage.SaveAs(caminhoParaImagem);
                        System.IO.File.Delete(Path.Combine(Server.MapPath("~/Imagens/Items/"), nomeVelho));
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Ocorreu o seguinte erro: " + e.Message);
                }
            }

            return View(item);
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
