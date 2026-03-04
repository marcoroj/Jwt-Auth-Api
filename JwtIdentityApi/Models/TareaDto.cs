using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JwtIdentityApi.Models
{
    public class TareaDto
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(maximumLength: 100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(maximumLength: 1000, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public required string Descripcion { get; set; }
        public bool Estado { get; set; }
        public int Prioridad { get; set; }
        public DateTime FechaCreacion { get; set; } 

        public DateTime FechaFin { get; set; }
        public required string UsuarioId { get; set; }
        public required string UsuarioEmail { get; set; }
    }
}
