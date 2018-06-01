using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoL_Builds.Models
{
    public class Utilizadores
    {

        //Construtor
        public Utilizadores()
        {
            //Iniciar a iCollection
            Comentarios = new HashSet<Comentarios>();
        }
        //Chave Primaria
        [Key]
        public int ID { get; set; }

        //Nome
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório!")] // O atributo "Nome" é de preenchimento obrigatório
        [RegularExpression("[A-ZÂÍ][a-záéíóúãõàèìòùâêîôûäëïöüç.]+(( | de | da | dos | d' |-)[A-ZÂÍ][a-záéíóúãõàèìòùâêîôûäëïöüç.]+){1,3}", ErrorMessage = "O nome apenas aceita letras. Cada palavra começa com maiúscula, seguido de minúsculas...")]
        public string Nome { get; set; }

        //data de nascimento
        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório!")] // O atributo "DataNascimento" é de preenchimento obrigatório
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }

        //genero
        [StringLength(40)]
        [Display(Name = "Género")]
        public string Genero { get; set; }

        
        [Display(Name = "Email Associado")]
        public string UserName { get; set; }
        // Ligacao para N Comentarios
        public virtual ICollection<Comentarios> Comentarios { get; set; }
    }
}