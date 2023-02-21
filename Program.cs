using System.IdentityModel.Tokens.Jwt;
using System.Text;
using backend;
using backend.ApiBehavior;
using backend.Filtros;
using backend.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


//------------------- SERVICIOS ------------------------------------

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options => {
    options.Filters.Add(typeof(FiltroDeExcepcion));
    options.Filters.Add(typeof(ParsearBadRequest));
}).ConfigureApiBehaviorOptions(BehaviorBadRequests.Parsear);
builder.Services.AddCors(options => {
    var frontendURL = configuration.GetValue<string>("frontend_url");
    options.AddDefaultPolicy(builder => {
        builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader()
        .WithExposedHeaders(new string[] {"cantidadTotalRegistros"});
    });
});
builder.Services.AddDbContext<ApplicationDbContext>((options) => {
    options.UseMySQL(configuration.GetConnectionString("defaultConnection")!);
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();

//Servicios: auth
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer((options) => 
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true, //Los tokens tendran un tiempo de expiracion
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["llavejwt"])
            
            ),
            ClockSkew = TimeSpan.Zero
        }
    );
builder.Services.AddAuthorization((options) => {
    options.AddPolicy("EsAdmin", policy => policy.RequireClaim("role", "admin"));
});


//------------------- Configure the HTTP request pipeline ------------------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
