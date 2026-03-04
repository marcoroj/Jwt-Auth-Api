using JwtIdentityApi.Models;
using Microsoft.AspNetCore.Identity;

namespace JwtIdentityApi.Servicios
{
    public interface IUsuariosService
    {
        Task<IdentityUser?> ObtenerUsuarioLogueado();
    }
}