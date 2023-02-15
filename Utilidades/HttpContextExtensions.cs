using Microsoft.EntityFrameworkCore;

namespace backend.Utilidades
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacionEnCabecera<T>(
            this HttpContext httpContext,
            IQueryable<T> queryable)
        /*Esto es necesario porque desde el frontend necesito saber la cantidad de registros que hay
        en la bd para determinar la cantidad de paginas. Podria devolverse en el body, pero es mejor
        mandarlo en la cabecera como metadata*/
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            double cantidad = await queryable.CountAsync(); //Devuelve la cant de registros de la tabla
            httpContext.Response.Headers.Add("cantidadTotalRegistros", cantidad.ToString());
        }
    }
}