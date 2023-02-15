namespace backend.Entidades
{
    public class Genero
    {
        public int id {get; set;}
        public string? nombre {get; set;}
        public List<PeliculasGeneros>? peliculasGeneros {get; set;}
    }
}