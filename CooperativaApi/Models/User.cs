using System.ComponentModel.DataAnnotations;

namespace CooperativaApi.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required] public string Nombre { get; set; } = null!;
        [Required, EmailAddress] public string Email { get; set; } = null!;
        [Required] public string PasswordHash { get; set; } = null!;
        [Required] public string Rol { get; set; } = "Socio"; // Admin, Socio
        public Socio? Socio { get; set; } // opcional relacion 1:1
    }
}