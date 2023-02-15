using System.ComponentModel.DataAnnotations;

namespace backend.Entidades
{
    public class PeliculasActores
    {
        public int peliculaId {get; set;}
        public int actorId {get; set;}
        public Pelicula? pelicula {get; set;}
        public Actor? actor {get; set;}
        [StringLength(maximumLength: 30)]
        public string? personaje {get; set;}
        public int orden {get; set;} //"Importancia" del personaje en la pelicula
    }
}