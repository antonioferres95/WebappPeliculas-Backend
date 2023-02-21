using Microsoft.AspNetCore.Mvc;

namespace backend.ApiBehavior
{
    public static class BehaviorBadRequests
    {
        public static void Parsear(ApiBehaviorOptions options)
        {
            options.InvalidModelStateResponseFactory = (actionContext) => {
                var response = new List<string>();
                foreach (var llave in actionContext.ModelState.Keys)
                {
                    {
                        foreach (var error in actionContext.ModelState[llave]!.Errors)
                        {
                            response.Add($"{llave}: {error.ErrorMessage}");
                        }
                    }
                }

                return new BadRequestObjectResult(response);
            };
        }
    }
}