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
            return View(utilizadores);
        }       
        // GET: Utilizadores/UtilizadoresDetails/5
        public ActionResult UtilizadoresDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utilizadores utilizador = db.Utilizadores.Find(id);
            if (utilizador == null)
            {
                return HttpNotFound();
            }
            return PartialView(utilizador);
        }

        // GET: Utilizadores/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Utilizadores/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Genero")] Utilizadores utilizador, string DataNasc)
        {
            //Transformar a string num datetime
            DateTime dataN = DateTime.Parse(DataNasc);
            //determinar id do novo utilizador
            int novoID = 0;
            //proteger a criacao de um novo id, determinar o numero de utilizadores da tabela
            if (db.Utilizadores.Count() == 0) {
                novoID = 1;
            }
            else  {
                novoID = db.Utilizadores.Max(u => u.ID)+1;
            }
            //atribuir o id do novo utilizador
            utilizador.ID = novoID;

            //atribuir a data de nascimento
            utilizador.DataNascimento = dataN;

            if (ModelState.IsValid)
            {

                db.Utilizadores.Add(utilizador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(utilizador);
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

        // POST: Utilizadores/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DataNascimento,Nome,Genero,UserName")] Utilizadores utilizador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilizador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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

        // POST: Utilizadores/EditMe/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMe([Bind(Include = "ID,DataNascimento,Nome,Genero,UserName")] Utilizadores utilizador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilizador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }
            return View(utilizador);
        }


        // GET: Utilizadores/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
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
            return View(utilizadores);
        }

        // POST: Utilizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Utilizadores utilizadores = db.Utilizadores.Find(id);
            db.Utilizadores.Remove(utilizadores);
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
