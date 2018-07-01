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
    public class ChampRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChampRoles
        public ActionResult Index()
        {
            return View(db.ChampRoles.ToList());
        }

        // GET: ChampRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChampRoles champRoles = db.ChampRoles.Find(id);
            if (champRoles == null)
            {
                return HttpNotFound();
            }
            return View(champRoles);
        }

        
        /// <summary>
        ///     GET: ChampRoles/Create     
        ///     Metodo que retorna para o index das roles do champion, 
        ///     pois não há necessidade da criação de novas roles, o seed já as tem todas
        /// </summary>
        /// <returns>
        ///     retorna para o index das roles do champion
        /// </returns>
        [Authorize]
        public ActionResult Create()
        {
            return RedirectToAction("Index", "ChampRoles");
        }

        /// <summary>
        ///     GET: ChampRoles/Edit/5
        ///     Metodo que retorna para o index das roles do champion, 
        ///     pois não há necessidade da edição das roles
        /// </summary>
        /// <param name="id">
        ///     id da role a editar
        /// </param>
        /// <returns>
        ///  retorna para o index das roles do champion
        /// </returns>
        public ActionResult Edit(int? id)
        {
            return RedirectToAction("Index", "ChampRoles");
        }
        
        /// <summary>
        ///     GET: ChampRoles/Delete/5
        ///     Metodo que retorna para o index das roles do champion, 
        ///     pois não há necessidade da eliminação das roles
        /// </summary>
        /// <param name="id">
        ///     id da role a eliminar
        /// </param>
        /// <returns>
        ///  retorna para o index das roles do champion
        /// </returns>
        public ActionResult Delete(int? id)
        {
            return RedirectToAction("Index", "ChampRoles");
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
