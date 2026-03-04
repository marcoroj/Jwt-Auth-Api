namespace JwtIdentityApi.Models
{
    public class RespuestaAutenticacionDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime Expiracion { get; set; }    
    }
}
