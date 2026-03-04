using JwtIdentityApi.Data;
using JwtIdentityApi.Entities;
using JwtIdentityApi.Models;
using JwtIdentityApi.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtIdentityApi.Controllers
{
    [ApiController]
    [Route("api/tareas")]
    [Authorize]
    
    public class TareasController:ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUsuariosService _usuariosService;

        public TareasController(ApplicationDbContext context,IUsuariosService usuariosService)
        {
            _context = context;
            _usuariosService = usuariosService;
        }


        [HttpGet]
        public async Task<ActionResult<List<TareaDto>>> GetTareas()
        {
            var tareas = await _context.Tareas.Include(x=>x.Usuario).ToListAsync();
            var tareasDto = new List<TareaDto>();
            foreach (var x in tareas)
            {
                var tarea = new TareaDto
                {
                    Id = x.Id,
                    Titulo=x.Titulo,
                    Descripcion=x.Descripcion,
                    Estado=x.Estado,
                    Prioridad=x.Prioridad,
                    FechaCreacion=x.FechaCreacion,
                    FechaFin=x.FechaFin,
                    UsuarioEmail=x.Usuario.Email,
                    UsuarioId=x.Usuario.Id
                };
                tareasDto.Add(tarea);
            }
            return Ok(tareasDto);
            
        }

        [HttpGet("{id:int}",Name ="ObtenerTarea")]
        public async Task<ActionResult<TareaDto>> GetTareaById(int id)
        {
            var tareaExiste=await _context.Tareas.Include(x=>x.Usuario).FirstOrDefaultAsync(x=>x.Id==id);
            if(tareaExiste is null)
            {
                return NotFound();
            }
            var tareaDto = new TareaDto
            {
                Id = tareaExiste.Id,
                Titulo = tareaExiste.Titulo,
                Descripcion = tareaExiste.Descripcion,
                Estado = tareaExiste.Estado,
                Prioridad = tareaExiste.Prioridad,
                FechaCreacion = tareaExiste.FechaCreacion,
                FechaFin = tareaExiste.FechaFin,
                UsuarioEmail = tareaExiste.Usuario.Email,
                UsuarioId = tareaExiste.Usuario.Id
            };

            return Ok(tareaDto);

        }


        [HttpPost]
        public async Task<ActionResult> PostTarea(TareaCreacionDto tareaCreacionDto)
        {
            var usuario = await _usuariosService.ObtenerUsuarioLogueado();

            if(usuario is null)
            {
                return NotFound();
            }

            var tareaDb = new Tarea
            {
                Titulo=tareaCreacionDto.Titulo.Trim(),
                Descripcion=tareaCreacionDto.Descripcion.Trim(),
                Prioridad=tareaCreacionDto.Prioridad,
                UsuarioId=usuario.Id
            };

            _context.Add(tareaDb);
            await _context.SaveChangesAsync();
            var tareaDto = new TareaDto
            {
                Id = tareaDb.Id,
                Titulo = tareaDb.Titulo,
                Descripcion = tareaDb.Descripcion,
                Estado = tareaDb.Estado,
                Prioridad = tareaDb.Prioridad,
                FechaCreacion = tareaDb.FechaCreacion,
                FechaFin = tareaDb.FechaFin,
                UsuarioEmail = tareaDb.Usuario.Email,
                UsuarioId = tareaDb.Usuario.Id
            };
            return CreatedAtRoute("ObtenerTarea", new {id=tareaDb.Id},tareaDto);

        }
    }
}
