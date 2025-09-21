using System.ComponentModel.DataAnnotations;

namespace CooperativaApi.Models
{
    public class Socio
    {
        public int Id { get; set; }
        [Required] public string NombreCompleto { get; set; } = null!;
        [Required] public string CI { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;

        public int ProfesionId { get; set; }
        public Profesion? Profesion { get; set; }

        public ICollection<Aportacion>? Aportaciones { get; set; }
        public ICollection<Prestamo>? Prestamos { get; set; }

        // Optional: link to User if socio can loguearse con el mismo user
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
