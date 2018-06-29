using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoL_Builds.Models
{
    public class Champions
    {
        //Construtor
        public Champions()
        {
            //Iniciar as iCollections
            ChampRoles = new HashSet<ChampRoles>();
            Builds = new HashSet<Builds>();
        }

        //Chave Primaria
        [Key]
        public int ID { get; set; }

        //Nome
        [Display(Name = "Nome do Champion")]
        public string Nome { get; set; }

        //Descricao
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        //Imagem
        public string Imagem { get; set; }

        //Ligacao com N Roles
        [Display(Name = "Roles")]
        public virtual ICollection<ChampRoles> ChampRoles { get; set; }
        
        //Ligacao com N Builds
        public virtual ICollection<Builds> Builds { get; set; }
    }
}