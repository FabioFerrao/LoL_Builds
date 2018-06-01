using LoL_Builds.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(LoL_Builds.Startup))]
namespace LoL_Builds
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            IniciaAplicacao();
        }
        private void IniciaAplicacao() {
            ApplicationDbContext db = new ApplicationDbContext();
            var utilizador = new Utilizadores();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            // criar a Role 'Administrador'
            if (!roleManager.RoleExists("Administrador"))
            {
                // não existe a 'role'
                // então, criar essa role
                var role = new IdentityRole();
                role.Name = "Administrador";
                roleManager.Create(role);
            }

            // criar a Role 'Moderador'
            if (!roleManager.RoleExists("Moderador"))
            {
                // não existe a 'role'
                // então, criar essa role
                var role = new IdentityRole();
                role.Name = "Moderador";
                roleManager.Create(role);
            }
        
            // criar um utilizador 'Administrador'
            var user = new ApplicationUser();
            user.UserName = "admin@admin.com";
            user.Email = "admin@admin.com";
            string userPWD = "123Qwe!";
            var chkUser = userManager.Create(user, userPWD);

            //Dados pessoais
            utilizador.Nome = "Fabio Andre";
            utilizador.DataNascimento = new DateTime(1993, 10, 01);
            utilizador.Genero = "Masculino";
            utilizador.UserName = user.UserName;

            db.Utilizadores.Add(utilizador);

            //Adicionar o Utilizador à respetiva Role-Utilizador-
            if (chkUser.Succeeded)
            {
                var result1 = userManager.AddToRole(user.Id, "Administrador");
            }
            //https://code.msdn.microsoft.com/ASPNET-MVC-5-Security-And-44cbdb97

        }
    }
}

