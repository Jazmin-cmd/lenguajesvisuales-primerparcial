using System.ComponentModel.DataAnnotations;

namespace CooperativaApi.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        [StringLength(150, ErrorMessage = "El email no puede exceder 150 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El CI es obligatorio.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El CI debe tener entre 5 y 20 caracteres.")]
        public string CI { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "La dirección debe tener entre 5 y 150 caracteres.")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
        [StringLength(15, ErrorMessage = "El teléfono no puede exceder 15 caracteres.")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La profesión es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una profesión válida.")]
        public int ProfesionId { get; set; }

        [StringLength(20, ErrorMessage = "El rol no puede exceder 20 caracteres.")]
        public string? Rol { get; set; }
    }
}

