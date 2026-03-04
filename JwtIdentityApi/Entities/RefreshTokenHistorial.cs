using Microsoft.AspNetCore.Identity;

namespace JwtIdentityApi.Entities
{
    public class RefreshTokenHistorial
    {
        public int Id { get; set; }
        public required string UsuarioId { get; set; }
        public required string Token {  get; set; }
        public required string RefreshToken { get; set; }
        public DateTime FechCreacion {  get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool EsActivo { get; private set; }
        public IdentityUser? Usuario { get; set; }



    }
}
