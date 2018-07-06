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
        
        [Key]
        public int ID { get; set; }

        //Nome descritivo da build criada
        [Required(ErrorMessage = "Nome da build necessária")]
        [RegularExpression("^(?![\x20.]+$)[a-zA-Z\x20.]*$", ErrorMessage = "O nome da build apenas aceita letras.")]
        [Display(Name ="Nome da build")]
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

        //Chave Forasteira para Utilizadores
        [ForeignKey("Utilizador")]
        [Display(Name = "Utilizador")]
        public int UtilizadorFK { get; set; }
        public virtual Utilizadores Utilizador { get; set; }
    }
}