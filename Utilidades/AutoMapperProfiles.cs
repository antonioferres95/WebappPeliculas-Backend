using AutoMapper;
using backend.DTOs;
using backend.Entidades;

namespace backend.Utilidades
{
    public class AutoMapperProfiles: Profile
    {   
        private List<PeliculasGeneros> MapearPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculasGeneros>();

            if (peliculaCreacionDTO.GenerosIds == null) {return resultado;}

            foreach (var id in peliculaCreacionDTO.GenerosIds)
            {
                resultado.Add(new PeliculasGeneros() {generoId = id});
            }

            return resultado;
        }

        private List<PeliculasActores> MapearPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculasActores>();

            if (peliculaCreacionDTO.Actores == null) {return resultado;}

            foreach (var actor in peliculaCreacionDTO.Actores)
            {
                resultado.Add(new PeliculasActores() {actorId = actor.id, personaje = actor.personaje});
            }

            return resultado;
        }

        private List<GeneroDTO> MapearPeliculasGeneros(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            var resultado = new List<GeneroDTO>();

            if (pelicula.peliculasGeneros != null)
            {
                foreach (var genero in pelicula.peliculasGeneros)
                {
                    resultado.Add(new GeneroDTO() {id=genero.generoId, nombre=genero.genero.nombre} );
                }
            }

            return resultado;
        }

        private List<PeliculaActorDTO> MapearPeliculasActores(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            var resultado = new List<PeliculaActorDTO>();

            if (pelicula.peliculasActores != null)
            {
                foreach (var actorPeliculas in pelicula.peliculasActores)
                {
                    resultado.Add(new PeliculaActorDTO()
                    {
                        id=actorPeliculas.actorId,
                        nombre=actorPeliculas.actor.nombre,
                        foto=actorPeliculas.actor.foto,
                        orden=actorPeliculas.orden,
                        personaje=actorPeliculas.personaje
                    });
                }
            }

            return resultado;
        }

        public AutoMapperProfiles()
        //Aqui van los mapeos
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<Genero, GeneroCreacionDTO>().ReverseMap();
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember((x) => x.foto, options => options.Ignore()); /*Ignoramos la foto porque tiene
                                                                        un tratamiento especial*/
            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember((x) => x.poster, options => options.Ignore()) /*Ignoramos el poster porque tiene
                                                                          un tratamiento especial*/
                .ForMember((x) => x.peliculasGeneros, options => options.MapFrom(MapearPeliculasGeneros))
                .ForMember((x) => x.peliculasActores, options => options.MapFrom(MapearPeliculasActores));

            CreateMap<Pelicula, PeliculaDTO>()
                .ForMember((x) => x.generos, options => options.MapFrom(MapearPeliculasGeneros))
                .ForMember((x) => x.actores, options => options.MapFrom(MapearPeliculasActores));
        }
    }
}