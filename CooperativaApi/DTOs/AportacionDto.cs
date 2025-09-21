using System.ComponentModel.DataAnnotations;
namespace CooperativaApi.DTOs
{
    public class AportacionDto
{
    public int Id { get; set; }

    [Range(350000, 1000000, ErrorMessage = "El monto debe estar entre 0.01 y 1.000.000.")]
    public decimal Monto { get; set; }

    public DateTime Fecha { get; set; }

    [StringLength(50, ErrorMessage = "El nombre del socio no puede exceder 100 caracteres.")]
    public string SocioNombre { get; set; }
}

}
