namespace backend.DTOs
{
    public class HomeDTO
    {
        public List<PeliculaDTO> enCines {get; set;}
        public List<PeliculaDTO> proximosEstrenos {get; set;}
    }
}