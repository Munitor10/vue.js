using System.ComponentModel.DataAnnotations;

namespace AlunosAPIv2.Models
{
    public class Pessoa
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="O campo nome e obrigatorio!")]
        [MinLength(4, ErrorMessage ="O nome deve ter no minimo 4 caractere.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo idade e obrigatorio!")]
        [Range(19, int.MaxValue, ErrorMessage ="A idade deve ser maior que 18.")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "O campo email e obrigatorio!")]
        [EmailAddress(ErrorMessage = "O email esta incorreto.")]
        public string Email { get; set; }
        public bool? Ativo { get; set; }
    }
}