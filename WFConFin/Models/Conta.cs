using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WFConFin.Models
{
    public class Conta
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Descricao { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }


        [Required]
        [DataType(DataType.Date)]
        public DateTime? DataVencimento { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? DataPagamento { get; set; }

        [Required]
        public Situacao Situacao { get; set; }

        public Guid PessoaId { get; set; }

        public Conta()
        {
            Id = new Guid();
        }

        //Relacionamento Entity
        public Pessoa Pessoa { get; set; }
    }
}
