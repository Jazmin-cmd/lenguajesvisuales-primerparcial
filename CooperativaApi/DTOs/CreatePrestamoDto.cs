using System.ComponentModel.DataAnnotations;

namespace CooperativaApi.DTOs
{
    public class CreatePrestamoDto
    {
        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(500000, double.MaxValue, ErrorMessage = "El monto debe ser mayor o igual a 1000.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El plazo en meses es obligatorio.")]
        [Range(1, 60, ErrorMessage = "El plazo debe estar entre 1 y 60 meses.")]
        public int PlazoMeses { get; set; }
    }
}
