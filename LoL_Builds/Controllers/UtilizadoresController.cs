using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LoL_Builds.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LoL_Builds.Controllers
{
    public class UtilizadoresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Utilizadores
        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            return View(db.Utilizadores.ToList());
        }

        // GET: Utilizadores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilizadores utilizadores = db.Utilizadores.Find(id);
            if (utilizadores == null)
            {
                return HttpNotFound();
            }
            
            return PartialView(utilizadores);
        }
        // GET: Utilizadores/UtilizadoresDetails/5
        public ActionResult UtilizadoresDetails(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Utilizadores");
            }
            Utilizadores utilizador = db.Utilizadores.Find(id);
            if (utilizador == null)
            {
                return RedirectToAction("Index", "Utilizadores");
            }
            return PartialView(utilizador);
        }


        // GET: Utilizadores/Edit/5

        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Utilizadores utilizadores = db.Utilizadores.Find(id);
            if (utilizadores == null)
            {
                return RedirectToAction("Index");
            }
            return View(utilizadores);
        }

        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        ///     POST: Utilizadores/Edit/5
        ///     Metodo que edita utilizadores
        /// </summary>
        /// <param name="utilizador">
        /// recebe o utilizador por parametro, mais propriamente o id, a data de nascimento, o nome, o genero e o username,
        /// com esses dados vai editar o utilizador adicionando mais alguns parametros
        /// </param>
        /// <returns>
        ///     retorna para o index depois de editar o utilizador
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DataNascimento,Nome,Genero,UserName")] Utilizadores utilizador)
        {
            string DataNasc = utilizador.DataNascimento.ToString("yyyy-MM-dd");
            //caso a data nao seja inserida pelo utilizador
            if (DataNasc == "")
            {
                return View(utilizador);
            }
            //Comparacao com a data de "agora"
            int comparacao = DateTime.Compare(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")), DateTime.Parse(DataNasc));

            //caso a data seja superior ou igual à atual
            if (comparacao <= 0)
            {
                return View(utilizador);
            }
            if (ModelState.IsValid)
            {
                db.Entry(utilizador).State = EntityState.Modified;
                db.SaveChanges();
                if (utilizador.UserName == User.Identity.Name) {
                    Session["nomeUser"] = utilizador.Nome;
                }
                return RedirectToAction("Details", "UsersAdmin", new { userName = Session["UserEmail"] });
            }
            return View(utilizador);
        }

        // GET: Utilizadores/EditMe/5

        [Authorize]
        public ActionResult EditMe(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Manage");
            }
            Utilizadores utilizador = db.Utilizadores.Find(id);
            if (utilizador == null || utilizador.UserName != User.Identity.GetUserName())
            {
                return RedirectToAction("Index", "Manage");
            }
            return View(utilizador);
        }

        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        ///     POST: Utilizadores/EditMe/5
        ///     Metodo que serve para o utilizador logado editar os seus dados, para nao aceder aos dados dos outros utilizadores
        /// </summary>
        /// <param name="utilizador">
        /// recebe o utilizador por parametro, mais propriamente o id, a data de nascimento, o nome, o genero e o username,
        /// com esses dados vai editar o utilizador adicionando mais alguns parametros
        /// </param>
        /// <returns>
        ///     retorna para o index depois de editar o utilizador
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMe([Bind(Include = "ID,DataNascimento,Nome,Genero,UserName")] Utilizadores utilizador)
        {
            string DataNasc = utilizador.DataNascimento.ToString("yyyy-MM-dd");
            //caso a data nao seja inserida pelo utilizador
            if (DataNasc == "")
            {
                return View(utilizador);
            }
            //Comparacao com a data de "agora"
            int comparacao = DateTime.Compare(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")), DateTime.Parse(DataNasc));

            //caso a data seja superior ou igual à atual
            if (comparacao <= 0)
            {
                return View(utilizador);
            }

            if (ModelState.IsValid)
            {
                db.Entry(utilizador).State = EntityState.Modified;
                db.SaveChanges();
                Session["nomeUser"] = utilizador.Nome;
                return RedirectToAction("Index", "Manage");
            }
            return View(utilizador);
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
