using System.ComponentModel.DataAnnotations;

namespace backend.Entidades
{
    public class ActorCreacionDTO
    {
        public string? nombre {get; set;}
        public string? biografia {get; set;}
        public DateTime fechaNacimiento {get; set;}
        public IFormFile? foto {get; set;}
    }
}