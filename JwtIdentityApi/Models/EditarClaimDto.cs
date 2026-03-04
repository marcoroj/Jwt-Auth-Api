using System.ComponentModel.DataAnnotations;

namespace JwtIdentityApi.Models
{
    public class EditarClaimDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
    }
}
