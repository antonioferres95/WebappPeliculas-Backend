using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace backend.Entidades
{
    public class Rating
    {
        public int id {get; set;}
        [Range(1,5)]
        public int puntuacion {get; set;}
        public int peliculaId {get; set;}
        public string? usuarioId {get; set;}
        public Pelicula? pelicula {get; set;}
        public IdentityUser? usuario {get; set;}
    }
}