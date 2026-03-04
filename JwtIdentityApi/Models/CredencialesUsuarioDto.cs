using System.ComponentModel.DataAnnotations;

namespace JwtIdentityApi.Models
{
    public class CredencialesUsuarioDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
        [Required]
        public  string? Password { get; set; }
    }
}
