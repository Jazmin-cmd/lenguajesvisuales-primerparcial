using System.Text.Json.Serialization;
using CooperativaApi.Models;

public class Profesion
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;

    [JsonIgnore] // Esto evita que se serialice la lista de socios
    public List<Socio> Socios { get; set; } = new();
}
