namespace CooperativaApi.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public int SocioId { get; set; }
        public Socio? Socio { get; set; }
        public decimal Monto { get; set; }
        public decimal Interes { get; set; } // porcentaje, ej. 12.5
        public int PlazoMeses { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Aprobado, Rechazado
        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
    }
}
