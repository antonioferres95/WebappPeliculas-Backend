using backend;
using backend.ApiBehavior;
using backend.Filtros;
using backend.Utilidades;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


// Add services to the container.
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
    options.UseMySQL(configuration.GetConnectionString("defaultConnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
