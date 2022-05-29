using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WFConFin.Models
{
    public class Pessoa
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Nome { get; set; }

        [StringLength(20)]
        public string Telefone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salario { get; set; }

        [Required]
        [StringLength(20, ErrorMessageResourceName = "Campo Genero deve ter até 20 caracteres")]
        public string Genero { get; set; }

        public Guid CidadeId { get; set; }

        public Pessoa()
        {
            Id = new Guid();
        }
        //Relacionamento Entity Framework

        public Cidade Cidade { get; set; }
    }
}
