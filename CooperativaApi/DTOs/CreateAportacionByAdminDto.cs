using System.ComponentModel.DataAnnotations;

namespace CooperativaApi.DTOs
{
    public class CreateAportacionByAdminDto
    {
        [Required(ErrorMessage = "El SocioId es obligatorio.")]
        public int SocioId { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, 1000000, ErrorMessage = "El monto debe estar entre 0.01 y 1.000.000.")]
        public decimal Monto { get; set; }
    }
}


