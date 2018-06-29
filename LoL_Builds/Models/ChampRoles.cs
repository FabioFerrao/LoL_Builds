using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoL_Builds.Models
{
    public class ChampRoles
    {
        //Construtor
        public ChampRoles()
        {
            //Iniciar a iCollection
            Champions = new HashSet<Champions>();
        }
        //Chave Primaria
        [Key]
        public int ID { get; set; }

        //Role(papel do champion na partida)
        public string Role { get; set; }

        //Imagem
        public string Imagem { get; set; }


        //Ligacao N-N com os Champions
        public virtual ICollection<Champions> Champions { get; set; }

    }
}