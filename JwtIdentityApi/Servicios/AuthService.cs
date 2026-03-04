using JwtIdentityApi.Data;
using JwtIdentityApi.Entities;
using JwtIdentityApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtIdentityApi.Servicios
{
    public class AuthService:IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthService(ApplicationDbContext context,UserManager<IdentityUser> userManager
            ,IConfiguration configuration,SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _config = configuration;
            _signInManager = signInManager;
        }

        public async Task<AuthRegistroResultadoDto> Registro(CredencialesUsuarioDto credencialesUsuarioDto)
        {
            var usuario = new IdentityUser
            {
                UserName = credencialesUsuarioDto.Email,
                Email = credencialesUsuarioDto.Email

            };
            var resultado = await _userManager.CreateAsync(usuario, credencialesUsuarioDto.Password);
            if (!resultado.Succeeded)
            {
                return new AuthRegistroResultadoDto
                {
                    Exitoso = false,
                    Errores = resultado.Errors.Select(x => x.Description).ToList()

                };
            }

            return new AuthRegistroResultadoDto
            {
                Exitoso = true,
                RespuestaAuhenticacion = new RespuestaAutenticacionDto
                {
                    AccessToken = await ConstruirToken(usuario),
                    RefreshToken = await GenerarYGuardarRefreshToken(usuario),
                }
            };
        }


        public async Task<string> GenerarYGuardarRefreshToken(IdentityUser usuario)
        {
            var refreshToken = GenerarRefreshToken();
            var refreshTokenHash=HashRefreshToken(refreshToken);
            var guardarRefreshToken = new RefreshToken
            {
                RefreshTokenHash = refreshTokenHash,
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddMinutes(2),
                EstaRevocado = false,
                UsuarioId = usuario.Id

            };
            _context.RefreshTokens.Add(guardarRefreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;

        }

        //GeNERANDO REFRESH TOKEN
        private string GenerarRefreshToken()
        {
            var randomNumber = new Byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<RespuestaAutenticacionDto?> Login(CredencialesUsuarioDto credencialesUsuarioDto)
        {
            var usuario = await _userManager.FindByEmailAsync(credencialesUsuarioDto.Email);

            if(usuario is null)
            {
                return null;
            }

            var resultado = await _signInManager.CheckPasswordSignInAsync(
                usuario, credencialesUsuarioDto.Password, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                var accesToken = await ConstruirToken(usuario);
                var refreshToken = await GenerarYGuardarRefreshToken(usuario);
                //var refreshToken = GenerarToken();
                //var guardarTokens = await GuardarHistorialRefreshToken(usuario.Id, token, refreshToken);
                return new RespuestaAutenticacionDto { AccessToken = accesToken, RefreshToken = refreshToken };

            }
            else
            {
                return null;
            }
        }

        private string HashRefreshToken(string refreshToken)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(refreshToken);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private async Task<RefreshToken?> ValidarRefreshToken(string usuarioId,string refreshToke)
        {
            var refreshTokenHash=HashRefreshToken(refreshToke);
            var buscarRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(
                x => x.RefreshTokenHash == refreshTokenHash && x.UsuarioId == usuarioId);
            if(buscarRefreshToken is null)
            {
                return null;
            }
            if(buscarRefreshToken.FechaExpiracion<=DateTime.UtcNow
                || buscarRefreshToken.EstaRevocado == true)
            {
                return null;
            }
            return buscarRefreshToken;


        }

        public async Task<RespuestaAutenticacionDto?> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var refreshTokenEsValido = await ValidarRefreshToken(refreshTokenRequestDto.UsuarioId,refreshTokenRequestDto.RefreshToken);
            if (refreshTokenEsValido is null)
            {
                return null;
            }
            
            var usuario = await _userManager.FindByIdAsync(refreshTokenRequestDto.UsuarioId);
            if (usuario == null)
            {
                return null;
            }
            refreshTokenEsValido.EstaRevocado = true;
            await _context.SaveChangesAsync();

            var tokens = new RespuestaAutenticacionDto
            {
                AccessToken = await ConstruirToken(usuario),
                RefreshToken = await GenerarYGuardarRefreshToken(usuario)
            };
            return tokens;
        }
        


        private async Task<string> ConstruirToken(IdentityUser usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier,usuario.Id),
                new Claim(ClaimTypes.Name,usuario.UserName!)   ,
                new Claim(ClaimTypes.Email,usuario.Email) ,
            };

            var claimsDb = await _userManager.GetClaimsAsync(usuario);
            claims.AddRange(claimsDb);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token"]!));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddMinutes(2);

            var construccionToken = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiracion,
                signingCredentials: credenciales
                );
            var token = new JwtSecurityTokenHandler().WriteToken(construccionToken);
            return token;

        }

        
    }
}
