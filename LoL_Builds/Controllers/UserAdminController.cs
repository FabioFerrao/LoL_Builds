﻿    using LoL_Builds.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LoL_Builds.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsersAdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {

            return View(await UserManager.Users.ToListAsync());
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string userName)
        {

            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = UserManager.FindByEmail(userName);
            if (user == null) {
                return RedirectToAction("Index", "Utilizadores");
            }

            var utilizadorArray = db.Utilizadores.Where(u => u.UserName.Equals(user.UserName));
            var i = 0;
            foreach (var u in utilizadorArray)
            {
                i = u.ID;
            }
            Session["UtilID"] = i;
            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

            var id = user.Id;
            Session["UserID"] = id;
            Session["UserEmail"] = userName;
            Session["MyEmail"] = User.Identity.GetUserName();
            return View(user);
        }

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user.Email == User.Identity.GetUserName())
            {
                return RedirectToAction("Details", "UsersAdmin", new { userName = Session["UserEmail"] });
            }
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.Email;
                user.Email = editUser.Email;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Details", "UsersAdmin", new { userName = Session["UserEmail"] });
            }
            ModelState.AddModelError("", "Algo falhou.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user.Email == User.Identity.GetUserName())
            {
                return RedirectToAction("Details", "UsersAdmin", new { userName = Session["UserEmail"] });
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        ///     POST: /Users/Delete/5
        ///     Metodo que elimina um utilizador, em ambos os "lados"
        /// </summary>
        /// <param name="id">
        ///     id do utilizador que é para eliminar
        /// </param>
        /// <returns>
        ///     retorna para o index, caso a eliminação seja aceite
        /// </returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {

                var user = await UserManager.FindByIdAsync(id);
                int idUtilizador = db.Utilizadores.Where(u => u.UserName.Equals(user.Email)).First().ID;
                Utilizadores utilizador = db.Utilizadores.Find(idUtilizador);

                //eliminacao dos comentarios relativos a esse utilizador
                while (utilizador.Comentarios.Count() != 0)
                {
                    db.Comentarios.Remove(utilizador.Comentarios.First());
                }

                utilizador.Comentarios = new List<Comentarios> { };




                //eliminacao das builds relativas a esse utilizador
                while (utilizador.Builds.Count() != 0)
                {

                    //eliminacao dos items associados a cada build
                    Builds build = db.Builds.Find(utilizador.Builds.First().ID);

                    while (build.Items.Count() != 0)
                    {
                        build.Items.First().Builds.Remove(build);
                        build.Items.Remove(build.Items.First());
                    }
                    build.Items = new List<Items> { };

                    db.Builds.Remove(utilizador.Builds.First());
                }

                utilizador.Builds = new List<Builds> { };


                utilizador.Builds.Count();


                db.Utilizadores.Remove(utilizador);
                db.SaveChanges();


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }


                return RedirectToAction("Index", "Utilizadores");
            }
            return View();
        }
    }
}
