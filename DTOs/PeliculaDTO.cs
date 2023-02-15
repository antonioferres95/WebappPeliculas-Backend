namespace backend.DTOs
{
    public class PeliculaDTO
    {
        public int id {get; set;}
        public string? titulo {get; set;}
        public string? resumen {get; set;}
        public string? trailer {get; set;}
        public bool enCines {get; set;}
        public DateTime fechaLanzamiento {get;set;}
        public string? poster {get; set;}
        public List<GeneroDTO>? generos {get; set;}
        public List<PeliculaActorDTO>? actores {get; set;}
    }
}