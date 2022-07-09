using System.ComponentModel.DataAnnotations;

namespace WFConFin.Models
{
    public class UsuarioLogin
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Password { get; set; }

    }
}
