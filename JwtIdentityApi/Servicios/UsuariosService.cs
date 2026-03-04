using JwtIdentityApi.Data;
using JwtIdentityApi.Entities;
using JwtIdentityApi.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace JwtIdentityApi.Servicios
{
    public class UsuariosService : IUsuariosService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public UsuariosService(UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor,ApplicationDbContext context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        

        

       

        //public async Task<RespuestaAutenticacionDto> GuardarHistorialRefreshToken(
        //    string idUsuario,string token,string refreshToken)
        //{
        //    var historialRefreshToken = new RefreshTokenHistorial
        //    {
        //        UsuarioId = idUsuario,
        //        Token = token,
        //        RefreshToken = refreshToken,
        //        FechCreacion = DateTime.UtcNow,
        //        FechaExpiracion = DateTime.UtcNow.AddDays(2),

        //    };

        //    await _context.RefreshTokensHistoriales.AddAsync(historialRefreshToken);
        //    await _context.SaveChangesAsync();
        //    return new RespuestaAutenticacionDto { AccessToken = token, RefreshToken = refreshToken };

        //}




        public async Task<IdentityUser?> ObtenerUsuarioLogueado()
        {
            var emailClaim = _httpContextAccessor.HttpContext!
                            .User.Claims.Where(x => x.Type == "email")
                            .FirstOrDefault();

            if (emailClaim is null)
            {
                return null;
            }
            var email = emailClaim.Value;

            var usuarioEncontrado=await _userManager.FindByEmailAsync(email);
            

            return usuarioEncontrado;


        }


    }
}
