namespace backend.DTOs
{
    public class PeliculasPutGetDTO
    {
        public PeliculaDTO pelicula {get; set;}
        public List<GeneroDTO> generosSeleccionados {get; set;}
        public List<GeneroDTO> generosNoSeleccionados {get; set;}
        public List<PeliculaActorDTO> actores {get; set;}
    }
}