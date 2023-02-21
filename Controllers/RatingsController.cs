using backend.DTOs;
using backend.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    public class RatingsController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext context;

        public RatingsController(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }
        
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //Con esto hacemos que solamente usuarios autenticados pueden usar este endpoint
        public async Task<ActionResult> Post([FromBody] RatingDTO ratingDTO)
        {
            var email = HttpContext.User.Claims.FirstOrDefault((x) => x.Type == "email")!.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var ratingActual = await context.Ratings!
                .FirstOrDefaultAsync((x) => x.peliculaId == ratingDTO.peliculaId && x.usuarioId == usuarioId);
            
            if(ratingActual == null)
            //Si el usuario no voto nunca esta pelicula
            {
                var rating = new Rating();
                rating.peliculaId = ratingDTO.peliculaId;
                rating.puntuacion = ratingDTO.puntuacion;
                rating.usuarioId = usuarioId;
                context.Add(rating);
            }
            else
            {
                ratingActual.puntuacion = ratingDTO.puntuacion;
            }

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}