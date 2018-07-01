namespace LoL_Builds.Migrations
{
    using LoL_Builds.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LoL_Builds.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LoL_Builds.ApplicationDbContext context)
        {

            //adicionar Items
            var items = new List<Items>{
               new Items {ID=1, Nome="Infinity Edge", Descricao="Item de Critical Strike e Damage", Imagem="InfinityEdge.png" },
               new Items {ID=2, Nome="Phantom Dancer", Descricao="Item de Critical Strike e Attack Speed", Imagem="PhantomDancer.png"},
               new Items {ID=3, Nome="Mortal Reminder", Descricao="Item de Damage e Armor Penetration" , Imagem="MortalReminder.png"},
               new Items {ID=4, Nome="Guardian Angel ", Descricao="Item de Defesa e de Damage", Imagem="GuardianAngel.png"},
               new Items {ID=5, Nome="Rabadon's Deathcap", Descricao="Item de Ability Power" , Imagem="RabadonDeathcap.png"},
               new Items {ID=6, Nome="Void Staff", Descricao="Item de Ability Power e de Magic Penetration", Imagem="VoidStaff.png"},
               new Items {ID=7, Nome="The Black Cleaver", Descricao="Item de Damage, Vida e Cooldown Reduction" , Imagem="BlackCleaver.png"},
               new Items {ID=8, Nome="Frozen Mallet", Descricao="Item de Damage e Vida ", Imagem="FrozenMallet.png"},
               new Items {ID=9, Nome="Thornmail", Descricao="Item de Defesa e Vida" , Imagem="Thornmail.png"},
               new Items {ID=10, Nome="Ardent Censer", Descricao="Item de Ability Power, Cooldown Reduction e Mana Regeneration", Imagem="ArdentCenser.png"},
            };
            items.ForEach(ii => context.Items.AddOrUpdate(i => i.Nome, ii));
            context.SaveChanges();

            //adicionar ChampRoles
            var ChampRoles = new List<ChampRoles> {
                new ChampRoles {ID=1, Role="SUPPORT", Imagem="Support.png"},
                new ChampRoles {ID=2, Role="BOT", Imagem="Bot.png"},
                new ChampRoles {ID=3, Role="MID", Imagem="Mid.png"},
                new ChampRoles {ID=4, Role="JUNGLER", Imagem="Jungler.png"},
                new ChampRoles {ID=5, Role="TOP", Imagem="Top.png"},
            };

            ChampRoles.ForEach(rr => context.ChampRoles.AddOrUpdate(r => r.Role, rr));
            context.SaveChanges();

            // adiciona Champions
            var champions = new List<Champions> {
                new Champions {ID=1, Nome="Ashe", Descricao="Champion ADC, preferencialmente jogado no BOT", Imagem="Ashe.png", ChampRoles = new List<ChampRoles>{ ChampRoles[1]} },
                new Champions {ID=2, Nome="Annie", Descricao="Champion jogado no BOT como Support ou no MID como AP Carry", Imagem="Annie.png", ChampRoles = new List<ChampRoles>{ ChampRoles[0], ChampRoles[2]} },
                new Champions {ID=3, Nome="Rengar", Descricao="Champion preferencialmente jogado como Jungler", Imagem="Rengar.png", ChampRoles = new List<ChampRoles>{ ChampRoles[3]} },
                new Champions {ID=4, Nome="Draven", Descricao="Champion ADC, preferencialmente jogado no BOT", Imagem="Draven.png", ChampRoles = new List<ChampRoles>{ ChampRoles[1]} },
                new Champions {ID=5, Nome="Lucian", Descricao="Champion ADC, preferencialmente jogado no BOT", Imagem="Lucian.png", ChampRoles = new List<ChampRoles>{ ChampRoles[1]} },
                new Champions {ID=6, Nome="Soraka", Descricao="Champion jogado no BOT como Support", Imagem="Soraka.png", ChampRoles = new List<ChampRoles>{ ChampRoles[0]} },
                new Champions {ID=7, Nome="Olaf", Descricao="Champion jogado no BOT como Support ou no MID como AP Carry", Imagem="Olaf.png", ChampRoles = new List<ChampRoles>{ ChampRoles[3], ChampRoles[4]} },
                new Champions {ID=8, Nome="Leblanc", Descricao="Champion jogado no MID como AP Carry", Imagem="LeBlanc.png", ChampRoles = new List<ChampRoles>{ChampRoles[2]} },
                new Champions {ID=9, Nome="Kayle", Descricao="Champion jogado no MID como AP Carry", Imagem="Kayle.png", ChampRoles = new List<ChampRoles>{ChampRoles[2]} },
                new Champions {ID=10, Nome="Lulu", Descricao="Champion jogado no BOT como Support", Imagem="Lulu.png", ChampRoles = new List<ChampRoles>{ ChampRoles[0]} },
            };
            champions.ForEach(cc => context.Champions.AddOrUpdate(c => c.Nome, cc));
            context.SaveChanges();

            // adiciona Utilizadores
            var utilizadores = new List<Utilizadores> {
               new Utilizadores {ID=1, Nome="Tânia Vieira", DataNascimento=new DateTime(1995,01, 21), Genero="Feminino", UserName="tania@tania.com"},
               new Utilizadores {ID=2, Nome="António Rocha",  DataNascimento=new DateTime(1992,12, 02), Genero="Masculino", UserName="antonio@antonio.com" },
               new Utilizadores {ID=3, Nome="André Silveira",  DataNascimento=new DateTime(1996,04, 05), Genero="Masculino", UserName="andre@andre.com"},
               new Utilizadores {ID=4, Nome="Lurdes Vieira",  DataNascimento=new DateTime(1992,01, 20), Genero="Feminino", UserName="lurdes@lurdes.com"},
               new Utilizadores {ID=5, Nome="Cláudia Pinto",  DataNascimento=new DateTime(1997,03, 21), Genero="Feminino", UserName="claudia@claudia.com" },
               new Utilizadores {ID=6, Nome="Rui Vieira",  DataNascimento=new DateTime(1989,11, 29), Genero="Masculino", UserName="rui@rui.com" },
               new Utilizadores {ID=7, Nome="Paulo Vieira",  DataNascimento=new DateTime(1997,11, 21), Genero="Masculino", UserName="paulo@paulo.com" },
               new Utilizadores {ID=8, Nome="Augusto Carvalho",  DataNascimento=new DateTime(1999,03, 15), Genero="Masculino", UserName="augusto@augusto.com" },
               new Utilizadores {ID=9, Nome="Beatriz Pinto",  DataNascimento=new DateTime(1995,01, 05), Genero="Feminino", UserName="beatriz@beatriz.com" },
               new Utilizadores {ID=10, Nome="José Alves", DataNascimento=new DateTime(1994,11, 18), Genero="Masculino", UserName="jose@jose.com" },
            };
            utilizadores.ForEach(uu => context.Utilizadores.AddOrUpdate(u => u.Nome, uu));
            context.SaveChanges();

            // adiciona Builds
            var builds = new List<Builds> {
              new Builds {ID=1, Nome="Power Draven", ChampionsFK=4, Items = new List<Items>{ items[0], items[1], items[2], items[3]}, UtilizadorFK=1},
              new Builds {ID=2, Nome="Oneshot Leblanc", ChampionsFK=8, Items = new List<Items>{ items[4], items[5]}, UtilizadorFK=2},
              new Builds {ID=3, Nome="Lethality Rengar", ChampionsFK=3, UtilizadorFK=3},
              new Builds {ID=4, Nome="Tank Olaf", ChampionsFK=7, Items = new List<Items>{ items[6], items[7], items[8]}, UtilizadorFK=1},
              new Builds {ID=5, Nome="Support Lulu", ChampionsFK=10, Items = new List<Items>{ items[9]}, UtilizadorFK=2},
              new Builds {ID=6, Nome="Speed Kayle", ChampionsFK=9, UtilizadorFK=4},
              new Builds {ID=7, Nome="Dps Lucian", ChampionsFK=5, UtilizadorFK=1},
              new Builds {ID=8, Nome="Crit Ashe", ChampionsFK=1, UtilizadorFK=1},
              new Builds {ID=9, Nome="Lethality Draven", ChampionsFK=4, UtilizadorFK=3},
            };
            builds.ForEach(bb => context.Builds.AddOrUpdate(b => b.Nome, bb));
            context.SaveChanges();

            // adiciona Comentarios
            var comentarios = new List<Comentarios> {
               new Comentarios {ID=1, Texto="Gostei bastante da build, porem acho que seria melhor com uma bloodthirster ", TimeStamp=new DateTime(2016,12, 01), UserID=2, BuildID=1 },
               new Comentarios {ID=2, Texto="Build perfeita", TimeStamp=new DateTime(2015,01, 21), UserID=3, BuildID=4 },
               new Comentarios {ID=3, Texto="Pena já não existir a death fire grasp :(", TimeStamp=new DateTime(2016,12, 25), UserID=4, BuildID=2 },
               new Comentarios {ID=4, Texto="lethality é bastante overpower", TimeStamp=new DateTime(2017,11, 21), UserID=6, BuildID=9 },
               new Comentarios {ID=5, Texto="Gostei, Overpowered!", TimeStamp=new DateTime(2018,02, 11), UserID=1, BuildID=9 },
               new Comentarios {ID=6, Texto="Amazing! Keep it up!", TimeStamp=new DateTime(2017,09, 01), UserID=6, BuildID=5 },
               new Comentarios {ID=7, Texto="Classic Lucian!", TimeStamp=new DateTime(2016,04, 11), UserID=7, BuildID=7 },
               new Comentarios {ID=8, Texto="One Shot Rengar!", TimeStamp=new DateTime(2018,03, 23), UserID=4, BuildID=3 },
               new Comentarios {ID=9, Texto="Muito Bom, aprovo esta build!", TimeStamp=new DateTime(2018,04, 25), UserID=1, BuildID=3 },
               new Comentarios {ID=10, Texto="A Riot deveria fazer alguma coisa em relação a este champion! ", TimeStamp=new DateTime(2016,12, 01), UserID=3, BuildID=3 },
            };
            comentarios.ForEach(cc => context.Comentarios.AddOrUpdate(c => c.ID, cc));
            context.SaveChanges();

        }

    }
}
