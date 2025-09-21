namespace CooperativaApi.DTOs
{
    public class SocioDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string CI { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string ProfesionNombre { get; set; } = string.Empty;
    }

}
