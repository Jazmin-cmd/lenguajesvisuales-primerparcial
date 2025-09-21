namespace CooperativaApi.DTOs
{
    public class LoginDataDto
    {
        public string Token { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public int Expires { get; set; } // en segundos
    }

}
