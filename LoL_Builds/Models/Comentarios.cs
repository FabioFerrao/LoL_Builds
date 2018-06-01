    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoL_Builds.Models
{
    public class Comentarios
    {

        //Chave Primaria
        [Key]
        public int ID { get; set; }

        //Texto
        public string Texto { get; set; }

        //TimeStamp 
            //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime TimeStamp { get; set; }

        //UserID
        [ForeignKey("Utilizador")]
        public int UserID { get; set; }
        public virtual Utilizadores Utilizador { get; set; }

        //BuildID   
        [ForeignKey("Build")]
        public int BuildID { get; set; }
        public virtual Builds Build { get; set; }


    }
}