//ASP.NET
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;


//Otros
using backend.Entidades;
using backend.DTOs;
using backend.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace backend.Controllers
{   
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    [Route("api/actores")]
    public class ActoresController: ControllerBase
    {
        private readonly ILogger<GenerosController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores"; //Nombre de la carpeta

        public ActoresController(
            ILogger<GenerosController> logger, 
            ApplicationDbContext context,
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO) 
        {
            var queryable = context.Actores!.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable); 
            var actores = await queryable.OrderBy((x) => x.nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<ActorDTO>>(actores); //Pasa del tipo Actor a ActorDTO,
                                                         //y retorna un ActorDTO
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActorDTO>> Get(int id)                                                                           
        {
            var actor = await context.Actores!.FirstOrDefaultAsync((x) => x.id == id);
            if(actor == null)
            {
                return NotFound();
            }
            else
            {
                return mapper.Map<ActorDTO>(actor);
            }
        }

        [HttpGet("buscarPorNombre/{nombre}")]
        public async Task<ActionResult<List<PeliculaActorDTO>>> BuscarPorNombre(string nombre = "")
        {
            if(string.IsNullOrWhiteSpace(nombre)) {return new List<PeliculaActorDTO>();}
            return await context.Actores!
                .Where((x) => x.nombre!.Contains(nombre))
                .OrderBy((x) => x.nombre)
                .Select((x) => new PeliculaActorDTO {id=x.id, nombre=x.nombre, foto=x.foto})
                .Take(5)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorParam)
        {
            var actor = mapper.Map<Actor>(actorParam);
            
            if(actorParam.foto != null)
            {
                actor.foto = await almacenadorArchivos.GuardarArchivo(contenedor, actorParam.foto);
            }

            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = await context.Actores!.FirstOrDefaultAsync((x) => x.id == id);
            if(actor == null)
            {
                return NotFound();
            }
            else
            {
                actor = mapper.Map(actorCreacionDTO, actor);

                if (actorCreacionDTO.foto != null)
                {
                    actor.foto = await almacenadorArchivos.EditarArchivo(actor.foto!, contenedor, actorCreacionDTO.foto);
                }

                await context.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var actor = await context.Actores!.FirstOrDefaultAsync((x) => x.id == id);
            if(actor == null)
            {
                return NotFound();
            }
            else
            {
                context.Remove(actor);
                await context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}