using System.ComponentModel.DataAnnotations;

namespace CooperativaApi.DTOs
{
    public class ProfesionDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la profesión es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre de la profesión debe tener entre 2 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
