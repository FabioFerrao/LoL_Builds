using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoL_Builds.Models
{
    public class Items
    {
        //Construtor
        public Items()
        {
            //Iniciar a iCollection
            Builds = new HashSet<Builds>();
        }   
        //Chave Primaria
        [Key]
        public int ID { get; set; }

        //Nome
        [Required(ErrorMessage = "Nome do item necessário")]
        [RegularExpression("^(?![\x20.]+$)[a-zA-Z'\x20.]*$", ErrorMessage = "O nome do item apenas aceita letras.")]
        public string Nome { get; set; }

        //Descricao
        [Required(ErrorMessage = "Descrição do item necessária")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        //Imagem
        [Required(ErrorMessage = "Imagem do item necessária")]
        public string Imagem { get; set; }

        //Ligacao N-N com as Builds
        [Display(Name = "Builds Associadas")]
        public virtual ICollection<Builds> Builds { get; set; }
    }
}