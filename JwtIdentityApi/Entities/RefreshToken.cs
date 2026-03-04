using Microsoft.AspNetCore.Identity;

namespace JwtIdentityApi.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public required string RefreshTokenHash { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool EstaRevocado { get; set; }
        public required string UsuarioId { get; set; }
        public IdentityUser? Usuario { get; set; }


    }
}
