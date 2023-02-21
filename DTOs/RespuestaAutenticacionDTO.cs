namespace backend.DTOs
{
    public class RespuestaAutenticacionDTO
    {
        public string? token {get; set;}
        public DateTime expiracion {get; set;}
    }
}