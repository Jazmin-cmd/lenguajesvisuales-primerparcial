using System.ComponentModel.DataAnnotations;

namespace CooperativaApi.DTOs
{
    public class CreateSocioWithUserDto
    {
        [Required, MaxLength(100)]
        public string NombreCompleto { get; set; }

        [Required, MaxLength(20)]
        public string CI { get; set; }

        [Required, MaxLength(150)]
        public string Direccion { get; set; }

        [Required, MaxLength(15)]
        public string Telefono { get; set; }

        [Required]
        public int ProfesionId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
    }

}
