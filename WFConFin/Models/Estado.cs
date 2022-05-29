using System.ComponentModel.DataAnnotations;

namespace WFConFin.Models
{
    public class Estado
    {
        [Key]
        [StringLength(2,MinimumLength = 2, ErrorMessage = "O Campo Sigla deve ter 2 caracteres")]
        public string Sigla { get; set; }

        [Required(ErrorMessage = "O Campo nome é Obrigatorio")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "O Campo nome deve ter 3 a 60 caracteres")]
        public string Nome { get; set; }
    }
}
