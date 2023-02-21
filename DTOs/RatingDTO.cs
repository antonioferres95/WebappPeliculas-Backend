using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class RatingDTO
    {
        public int peliculaId {get; set;}
        [Range(1,5)]
        public int puntuacion {get; set;}
    }
}