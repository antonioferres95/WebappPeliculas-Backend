using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{   
    public class CredencialesUsuarioDTO
    {
        [EmailAddress]
        [Required]
        public string? email {get; set;}
        [Required]
        public string? password {get; set;}
    }
}