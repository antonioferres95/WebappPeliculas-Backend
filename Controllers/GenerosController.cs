//ASP.NET
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;


//Otros
using backend.Entidades;
using backend.DTOs;
using backend.Utilidades;

namespace backend.Controllers
{   
    [ApiController]
    [Route("api/generos")]
    public class GenerosController: ControllerBase
    {
        private readonly ILogger<GenerosController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenerosController(
            ILogger<GenerosController> logger, 
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet] //Con paginacion
        public async Task<ActionResult<List<GeneroDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO) 
        {
            var queryable = context.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable); 
            var generos = await queryable.OrderBy((x) => x.nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<GeneroDTO>>(generos); //Pasa del tipo Genero a GeneroDTO,
                                                         //y retorna un GeneroDTO
        }

        [HttpGet("todos")] //Sin paginacion
        public async Task<ActionResult<List<GeneroDTO>>> Todos() 
        {
            var generos = await context.Generos.OrderBy((x) => x.nombre).ToListAsync();
            return mapper.Map<List<GeneroDTO>>(generos);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)                                                                           
        {
            var genero = await context.Generos.FirstOrDefaultAsync((x) => x.id == id);
            if(genero == null)
            {
                return NotFound();
            }
            else
            {
                return mapper.Map<GeneroDTO>(genero);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoParam)
        {
            var genero = mapper.Map<Genero>(generoParam);
            context.Add(genero);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = await context.Generos.FirstOrDefaultAsync((x) => x.id == id);
            if(genero == null)
            {
                return NotFound();
            }
            else
            {
                genero = mapper.Map(generoCreacionDTO, genero);
                await context.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync((x) => x.id == id);
            if(genero == null)
            {
                return NotFound();
            }
            else
            {
                context.Remove(genero);
                await context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}