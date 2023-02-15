using System.ComponentModel.DataAnnotations;
using backend.Validaciones;

namespace backend.DTOs
{
    public class GeneroCreacionDTO
    {
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(maximumLength: 15, ErrorMessage = "La longitud maxima es de 15 caracteres")]
        [PrimeraLetraMayusAttribute]
        public string? nombre {get; set;}
    }
}