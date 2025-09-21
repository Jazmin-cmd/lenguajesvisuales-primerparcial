namespace CooperativaApi.DTOs
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public object Data { get; set; } = null!;
    }
}
