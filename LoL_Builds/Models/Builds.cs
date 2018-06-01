using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoL_Builds.Models
{
    public class Builds
    {
        //Construtor
        public Builds()
        {
            //Iniciar as iCollections
            Items = new HashSet<Items>();
            Comentarios = new HashSet<Comentarios>();
        }

        //Chave Primária
        [Key]
        public int ID { get; set; }

        //Nome
        [Display(Name ="Nome da Build")]
        public string Nome { get; set; }

        //Chave Forasteira para Champions
        [ForeignKey("Champion")]
        [Display(Name ="Champion")]
        public int ChampionsFK { get; set; } 
        public virtual Champions Champion { get; set; }

        // Ligacao para N Items
        public virtual ICollection<Items> Items { get; set; }

        // Ligacao para N Comentarios
        public virtual ICollection<Comentarios> Comentarios { get; set; }

       
    }
}