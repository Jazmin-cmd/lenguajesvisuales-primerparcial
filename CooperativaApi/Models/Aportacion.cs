namespace CooperativaApi.Models
{
    public class Aportacion
    {
        public int Id { get; set; }
        public int SocioId { get; set; }         // FK a Socios
        public Socio? Socio { get; set; }        // Navegación
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}

