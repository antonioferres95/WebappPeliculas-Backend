using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using backend.DTOs;
using backend.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers
{   
    [Route("api/cuentas")]
    [ApiController]
    public class CuentasController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CuentasController(
            UserManager<IdentityUser> userManager, 
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.context = context;
            this.mapper = mapper;
        }

        private async Task<RespuestaAutenticacionDTO> ConstruirToken(CredencialesUsuarioDTO credenciales)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credenciales.email!),

            };

            var usuario =  await userManager.FindByEmailAsync(credenciales.email);
            var claimsDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]!));
            //llavejwt está en app.settings.Development.json
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1); //El token dura un año

            var token = new JwtSecurityToken(
                issuer:null, 
                audience:null, 
                claims: claims, 
                expires: expiracion,
                signingCredentials: creds);

            return new RespuestaAutenticacionDTO() {
                token= new JwtSecurityTokenHandler().WriteToken(token),
                expiracion=expiracion
            };
        }

        [HttpPost("crear")] 
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Crear ([FromBody] CredencialesUsuarioDTO credenciales)
        //Endpoint de registro
        {
            var usuario = new IdentityUser {UserName=credenciales.email, Email=credenciales.email};
            var resultado = await userManager.CreateAsync(usuario, credenciales.password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credenciales);
            }
            else 
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("login")] 
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Login ([FromBody] CredencialesUsuarioDTO credenciales)
        //Endpoint de login
        {
            var resultado = await signInManager.PasswordSignInAsync(
                credenciales.email,
                credenciales.password,
                isPersistent:false,
                lockoutOnFailure:false);
            
            if (resultado.Succeeded)
            {
                return await ConstruirToken(credenciales);
            }
            else
            {
                return BadRequest("No se pudo iniciar sesión");
            }
        }

        [HttpGet("listadoUsuarios")]
        public async Task<ActionResult<List<UsuarioDTO>>> ListadoUsuarios([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Users.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var usuarios = await queryable.OrderBy((x) =>x.Email).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<UsuarioDTO>>(usuarios);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPost("hacerAdmin")]
        public async Task<ActionResult> HacerAdmin([FromBody] string usuarioId)
        {
            var usuario = await userManager.FindByIdAsync(usuarioId);
            await userManager.AddClaimAsync(usuario, new Claim("role", "admin"));
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPost("removerAdmin")]
        public async Task<ActionResult> RemoverAdmin([FromBody] string usuarioId)
        {
            var usuario = await userManager.FindByIdAsync(usuarioId);
            await userManager.RemoveClaimAsync(usuario, new Claim("role", "admin"));
            return NoContent();
        }
    }
}