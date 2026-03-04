using JwtIdentityApi.Data;
using JwtIdentityApi.Entities;
using JwtIdentityApi.Models;
using JwtIdentityApi.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;

namespace JwtIdentityApi.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUsuariosService _usuariosService;
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public UsuariosController(UserManager<IdentityUser> userManager, IConfiguration configuration
            ,SignInManager<IdentityUser> signInManager,IUsuariosService usuariosService,ApplicationDbContext context,
            IAuthService authService)
        {
            _userManager = userManager;
            _config = configuration;
            _signInManager = signInManager;
            _usuariosService = usuariosService;
            _context = context;
            _authService = authService;
        }


        [HttpGet]
        [Authorize(Policy ="admin")]
        public async Task<IEnumerable<UsuarioDto>> ListarUsuarios()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            var usuariosDto=new List<UsuarioDto>();
            foreach(var x in usuarios)
            {
                usuariosDto.Add(new UsuarioDto { Email = x.Email!, Telefono = x.PhoneNumber });
            }
            return usuariosDto;
        }


        [HttpPost("registro")]
        public async Task<ActionResult<AuthRegistroResultadoDto>> RegistrarUsuario(CredencialesUsuarioDto credencialesUsuarioDto)
        {
            var resultado = await _authService.Registro(credencialesUsuarioDto);

            if (!resultado.Exitoso)
            {
                foreach(var x in resultado.Errores)
                {
                    ModelState.AddModelError(string.Empty, x);
                }
                return ValidationProblem();
            }
            return resultado;

        }

       



        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacionDto>> Login(CredencialesUsuarioDto credencialesUsuarioDto)
        {

            var usuario=await _authService.Login(credencialesUsuarioDto);
            if(usuario is null)
            {
                return RetornarLoginIncorrecto();
            }
            return Ok(usuario);
            

        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ActionResult<RespuestaAutenticacionDto>> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var refreshTokenEsValido=await _authService.RefreshToken(refreshTokenRequestDto);
            if(refreshTokenEsValido is null || refreshTokenEsValido.AccessToken is null || 
                refreshTokenEsValido.RefreshToken is null 
                )
            {
                return Unauthorized("Refresh Token invalido");
            }
            return refreshTokenEsValido;

        }

        private ActionResult RetornarLoginIncorrecto()
        {
            ModelState.AddModelError(string.Empty, "Login incorrecto");
            return ValidationProblem();
        }

        [HttpPost("hacer-admin")]
        //[Authorize(Policy = "admin")]
        public async Task<ActionResult> HacerAdmin(EditarClaimDto editarClaimDto)
        {
            var usuario = await _userManager.FindByEmailAsync(editarClaimDto.Email);
            if (usuario is null)
            {
                return NotFound();
            }
            await _userManager.AddClaimAsync(usuario, new Claim("admin","true"));
            return NoContent();
        }

        [HttpPost("remover-admin")]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult> RemoverAdmin(EditarClaimDto editarClaimDto)
        {
            var usuario = await _userManager.FindByEmailAsync(editarClaimDto.Email);
            if(usuario is null)
            {
                return NotFound();
            }
            await _userManager.RemoveClaimAsync(usuario, new Claim("admin", "true"));
            return NoContent();
        }

      

    }
}
