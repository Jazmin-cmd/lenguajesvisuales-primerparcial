namespace CooperativaApi.DTOs
{
    public class CreateUserDto
    {
        public string NombreCompleto { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

}
