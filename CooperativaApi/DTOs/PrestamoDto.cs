namespace CooperativaApi.DTOs
{
    public class PrestamoDto
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public int PlazoMeses { get; set; }
        public string Estado { get; set; } // "Pendiente", "Aprobado", etc.
        public string SocioNombre { get; set; } // Relación con Socio para mostrar solo el nombre
    }

}
