using JwtIdentityApi.Models;
using Microsoft.AspNetCore.Identity;

namespace JwtIdentityApi.Servicios
{
    public interface IAuthService
    {
        Task<AuthRegistroResultadoDto> Registro(CredencialesUsuarioDto credencialesUsuarioDto);
        Task<RespuestaAutenticacionDto?> Login(CredencialesUsuarioDto credencialesUsuarioDto);
        Task<RespuestaAutenticacionDto?> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto);
    }
}
