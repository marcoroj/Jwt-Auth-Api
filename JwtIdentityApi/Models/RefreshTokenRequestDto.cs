namespace JwtIdentityApi.Models
{
    public class RefreshTokenRequestDto
    {
        public required string UsuarioId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
