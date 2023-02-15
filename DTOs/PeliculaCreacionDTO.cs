using System.ComponentModel.DataAnnotations;
using backend.DTOs;
using backend.Utilidades;
using Microsoft.AspNetCore.Mvc;

namespace backend.Entidades
{
    public class PeliculaCreacionDTO
    {
        [Required]
        [StringLength(maximumLength:100)]
        public string? titulo {get; set;}
        public string? resumen {get; set;}
        public string? trailer {get; set;}
        public bool enCines {get; set;}
        public DateTime fechaLanzamiento {get;set;}
        public IFormFile? poster {get; set;}
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))] //Esto del modelbinder se hace porque debo recibir un listado de generos usando fromform (por tener un archivo)
        public List<int>? GenerosIds {get; set;}
        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculaCreacionDTO>>))] //Este es mas complejo porque la relacion tiene la propiedad personaje
        public List<ActorPeliculaCreacionDTO>? Actores {get; set;}
    }
}