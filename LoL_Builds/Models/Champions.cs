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
        [Required(ErrorMessage = "Nome do champion necessário")]
        [RegularExpression("^(?![\x20.]+$)[a-zA-Z'\x20.]*$", ErrorMessage = "O nome do champion apenas aceita letras.")]
        [Display(Name = "Nome do Champion")]
        public string Nome { get; set; }

        //Descricao
        [Required(ErrorMessage = "Descrição do champion necessária")]
        [Display(Name = "Descrição")]
        [StringLength(100)]
        public string Descricao { get; set; }

        //Imagem
        public string Imagem { get; set; }

        //Ligacao com N Roles
        [Required(ErrorMessage = "Pelo menos uma role do champion necessária")]
        [Display(Name = "Roles")]
        public virtual ICollection<ChampRoles> ChampRoles { get; set; }
        
        //Ligacao com N Builds
        public virtual ICollection<Builds> Builds { get; set; }
    }
}