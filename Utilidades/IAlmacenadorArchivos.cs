namespace backend.Utilidades
{
    public interface IAlmacenadorArchivos
    {
        Task BorrarArchivo(string ruta, string contenedor);
        Task<string> EditarArchivo(string ruta, string contenedor, IFormFile archivo);
        Task<string> GuardarArchivo(string contenedor, IFormFile archivo);
    }
}