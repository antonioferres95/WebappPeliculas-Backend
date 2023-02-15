using Microsoft.EntityFrameworkCore;
using backend.Entidades;

namespace backend
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeliculasActores>()
                .HasKey((x) => new {x.actorId, x.peliculaId}); //Defino la PK de PeliculasActores
            modelBuilder.Entity<PeliculasGeneros>()
                .HasKey((x) => new {x.generoId, x.peliculaId}); //Defino la PK de PeliculasGeneros
            
            base.OnModelCreating(modelBuilder);
        }
        

        public DbSet<Genero>? Generos {get; set;} /*Esto crea una tabla Generos cuyos campos
                                                 se van a basar en el modelo Genero*/

        public DbSet<Actor>? Actores {get; set;}
        public DbSet<Pelicula>? Peliculas {get; set;}
        public DbSet<PeliculasActores>? PeliculasActores {get; set;}
        public DbSet<PeliculasGeneros>? PeliculasGeneros {get; set;}
    }
}