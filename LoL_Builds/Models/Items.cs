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
        public string Nome { get; set; }

        //Descricao
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        //Imagem
        public string Imagem { get; set; }

        //Ligacao N-N com as Builds
        [Display(Name = "Builds Associadas")]
        public virtual ICollection<Builds> Builds { get; set; }
    }
}