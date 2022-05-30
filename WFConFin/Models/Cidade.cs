using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WFConFin.Models
{


    public class Cidade
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatorio")]

        [StringLength(2, MinimumLength = 2, ErrorMessage = "O Campo Sigla deve ter 2 caracteres")]
        public string EstadoSigla { get; set; }

        public Cidade()
        {
            Id = Guid.NewGuid();
        }

        [JsonIgnore]

        //Relacionamento Entity Framework
        public Estado Estado { get; set; }
    }
}
