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
                return RedirectToAction("Index", "ChampRoles");
            }
            ChampRoles champRole = db.ChampRoles.Find(id);
            if (champRole == null)
            {
                return RedirectToAction("Index", "ChampRoles");
            }
            return View(champRole);
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
