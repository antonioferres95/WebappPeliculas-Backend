using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace backend.Filtros
{
    public class ParsearBadRequest : IActionFilter
    //Esta clase convierte en un array todos los errores de un 400 badrequest
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var casteoResult = context.Result as IStatusCodeActionResult;
            
            if (casteoResult == null)
            {
                return;
            }

            var statusCode = casteoResult.StatusCode;
            if (statusCode == 400)
            {
                var response = new List<string>();
                var resultadoActual = context.Result as BadRequestObjectResult;
                if(resultadoActual.Value is string)
                {
                    response.Add(resultadoActual.Value.ToString());
                }
                else
                {
                    foreach (var llave in context.ModelState.Keys)
                    {
                        foreach (var error in context.ModelState[llave].Errors)
                        {
                            response.Add($"{llave}: {error.ErrorMessage}");
                        }
                    }
                }

                context.Result = new BadRequestObjectResult(response);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}