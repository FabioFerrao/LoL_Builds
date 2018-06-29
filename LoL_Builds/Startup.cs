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
        private void IniciaAplicacao()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var utilizador1 = new Utilizadores();
            var utilizador2 = new Utilizadores();
            var utilizador3 = new Utilizadores();
            var utilizador4 = new Utilizadores();
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
            
            // criar a Role 'Registado'
            if (!roleManager.RoleExists("Registado"))
            {
                // não existe a 'role'
                // então, criar essa role
                var role = new IdentityRole();
                role.Name = "Registado";
                roleManager.Create(role);
            }

            // criar um utilizador 'Administrador'
            var user1 = new ApplicationUser();
            user1.UserName = "admin@admin.com";
            user1.Email = "admin@admin.com";
            string user1PWD = "123Qwe!";
            var chkUser1 = userManager.Create(user1, user1PWD);

            //Dados pessoais
            utilizador1.Nome = "Fabio Andre";
            utilizador1.DataNascimento = new DateTime(1993, 10, 01);
            utilizador1.Genero = "Masculino";
            utilizador1.UserName = user1.UserName;
            db.Utilizadores.Add(utilizador1);


            //Adicionar o Utilizador à respetiva Role-Utilizador-
            if (chkUser1.Succeeded)
            {
                var result1 = userManager.AddToRole(user1.Id, "Administrador");
            }

            // criar um utilizador 'Moderador'
            var user2 = new ApplicationUser();
            user2.UserName = "mod1@mod1.com";
            user2.Email = "mod1@mod1.com";
            string user2PWD = "123Qwe!";
            var chkUser2 = userManager.Create(user2, user2PWD);

            //Dados pessoais
            utilizador2.Nome = "Mod Mod";
            utilizador2.DataNascimento = new DateTime(1998, 11, 23);
            utilizador2.Genero = "Masculino";
            utilizador2.UserName = user2.UserName;
            db.Utilizadores.Add(utilizador2);


            //Adicionar o Utilizador à respetiva Role-Utilizador-
            if (chkUser2.Succeeded)
            {
                var result2 = userManager.AddToRole(user2.Id, "Moderador");
            }


            // criar um utilizador 'Registado1'
            var user3 = new ApplicationUser();
            user3.UserName = "reg1@reg1.com";
            user3.Email = "reg1@reg1.com";
            string user3PWD = "123Qwe!";
            var chkUser3 = userManager.Create(user3, user3PWD);

            //Dados pessoais
            utilizador3.Nome = "Reg Reg";
            utilizador3.DataNascimento = new DateTime(1989, 03, 23);
            utilizador3.Genero = "Masculino";
            utilizador3.UserName = user3.UserName;
            db.Utilizadores.Add(utilizador3);


            //Adicionar o Utilizador à respetiva Role-Utilizador-
            if (chkUser3.Succeeded)
            {
                var result3 = userManager.AddToRole(user3.Id, "Registado");
            }

            // criar um utilizador 'Registado2'
            var user4 = new ApplicationUser();
            user4.UserName = "reg2@reg2.com";
            user4.Email = "reg2@reg2.com";
            string user4PWD = "123Qwe!";
            var chkUser4 = userManager.Create(user4, user4PWD);

            //Dados pessoais
            utilizador4.Nome = "Regg Regg";
            utilizador4.DataNascimento = new DateTime(1999, 03, 23);
            utilizador4.Genero = "Masculino";
            utilizador4.UserName = user4.UserName;
            db.Utilizadores.Add(utilizador4);


            //Adicionar o Utilizador à respetiva Role-Utilizador-
            if (chkUser4.Succeeded)
            {
                var result4 = userManager.AddToRole(user4.Id, "Registado");
            }

            //https://code.msdn.microsoft.com/ASPNET-MVC-5-Security-And-44cbdb97

        }
    }
}