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
    [Route("api/peliculas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class PeliculasController: ControllerBase
    {
        private readonly ILogger<GenerosController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas"; //Nombre de la carpeta

        public PeliculasController(
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
        [AllowAnonymous]
        public async Task<ActionResult<HomeDTO>> Get()
        {
            var top=6;
            var hoy= DateTime.Today;

            var proximosEstrenos = await context.Peliculas!
                .Where((x) => x.fechaLanzamiento > hoy)
                .OrderBy((x) => x.fechaLanzamiento)
                .Take(top)
                .ToListAsync();

            var enCines = await context.Peliculas!
                .Where((x) => x.enCines)
                .OrderBy((x) => x.fechaLanzamiento)
                .Take(top)
                .ToListAsync();

            var resultado = new HomeDTO();
            resultado.proximosEstrenos = mapper.Map<List<PeliculaDTO>>(proximosEstrenos);
            resultado.enCines = mapper.Map<List<PeliculaDTO>>(enCines);

            return resultado;
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)                                                                           
        {
            var pelicula = await context.Peliculas!
                .Include((x) => x.peliculasGeneros)!.ThenInclude((x) => x.genero)
                .Include((x) => x.peliculasActores)!.ThenInclude((x) => x.actor)
                .FirstOrDefaultAsync((x) => x.id == id);

            if(pelicula == null) {return NotFound();}

            var dto = mapper.Map<PeliculaDTO>(pelicula);    
            dto.actores = dto.actores!.OrderBy((x) => x.orden).ToList();
            return dto;
        }

        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] PeliculasFiltrarDTO peliculasFiltrarDTO)
        {
            var peliculasQueryable = context.Peliculas!.AsQueryable();

            if(!string.IsNullOrEmpty(peliculasFiltrarDTO.titulo))
            {
                peliculasQueryable = peliculasQueryable.Where((x) => x.titulo!.Contains(peliculasFiltrarDTO.titulo));
            }

            if(peliculasFiltrarDTO.enCines)
            {
                peliculasQueryable = peliculasQueryable.Where((x) => x.enCines);
            }

            if(peliculasFiltrarDTO.proximosEstrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where((x) => x.fechaLanzamiento > hoy);
            }

            if(peliculasFiltrarDTO.generoId != 0)
            {
                peliculasQueryable = peliculasQueryable
                    .Where((x) => x.peliculasGeneros!.Select((y) => y.generoId)
                    .Contains(peliculasFiltrarDTO.generoId));
            }

            await HttpContext.InsertarParametrosPaginacionEnCabecera(peliculasQueryable);

            var peliculas = await peliculasQueryable.Paginar(peliculasFiltrarDTO.paginacionDTO).ToListAsync();

            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        private void EscribirOrdenActores(Pelicula pelicula)
        {
            if (pelicula.peliculasActores != null)
            {
                for (int i = 0; i < pelicula.peliculasActores.Count; i++)
                {
                    pelicula.peliculasActores[i].orden = i; //El front me manda los actores en orden
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaParam)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaParam);
            if(peliculaParam.poster != null)
            {
                pelicula.poster = await almacenadorArchivos.GuardarArchivo(contenedor, peliculaParam.poster);
            }
            EscribirOrdenActores(pelicula);
            context.Add(pelicula);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("PostGet")]
        public async Task<ActionResult<PeliculasPostGetDTO>> PostGet()
        {
            var generos = await context.Generos!.ToListAsync();
            var generosDTO = mapper.Map<List<GeneroDTO>>(generos);

            return new PeliculasPostGetDTO() {Generos = generosDTO};
        }

        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<PeliculasPutGetDTO>> PutGet(int id)
        {
            var peliculaActionResult = await Get(id);
            if (peliculaActionResult.Result is NotFoundResult) {return NotFound();}

            var pelicula = peliculaActionResult.Value;

            var generosSeleccionadosIds = pelicula!.generos!.Select((x) => x.id).ToList();
            var generosNoSeleccionados = await context.Generos!
                .Where((x) => !generosSeleccionadosIds.Contains(x.id))
                .ToListAsync();

            var generosNoSeleccionadosDTO = mapper.Map<List<GeneroDTO>>(generosNoSeleccionados);

            var respuesta = new PeliculasPutGetDTO();
            respuesta.pelicula = pelicula;
            respuesta.generosSeleccionados = pelicula.generos;
            respuesta.generosNoSeleccionados = generosNoSeleccionadosDTO;
            respuesta.actores = pelicula.actores;

            return respuesta;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = await context.Peliculas!
                .Include((x) => x.peliculasActores)
                .Include((x) => x.peliculasGeneros)
                .FirstOrDefaultAsync((x) => x.id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            pelicula = mapper.Map(peliculaCreacionDTO, pelicula);

            if (peliculaCreacionDTO.poster != null)
            {
                pelicula.poster = await almacenadorArchivos.EditarArchivo(contenedor, pelicula.poster!, peliculaCreacionDTO.poster);
            }

            EscribirOrdenActores(pelicula);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var pelicula = await context.Peliculas!.FirstOrDefaultAsync((x) => x.id == id);
            if(pelicula == null)
            {
                return NotFound();
            }
            else
            {
                context.Remove(pelicula);
                await context.SaveChangesAsync();
                await almacenadorArchivos.BorrarArchivo(pelicula.poster!, contenedor);
                return NoContent();
            }
        }
    }
}