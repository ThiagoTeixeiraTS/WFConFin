using System.ComponentModel.DataAnnotations;

namespace WFConFin.Models
{
    public class Usuario
    {

    
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Nome { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Password { get; set; }


        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Funcao { get; set; }


        public Usuario()
        {
            Id = new Guid();
        }

    }
}
