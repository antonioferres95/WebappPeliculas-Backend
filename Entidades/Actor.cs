namespace backend.Entidades
{
    public class Actor
    {
        public int id {get; set;}
        public string? nombre {get; set;}
        public string? biografia {get; set;}
        public DateTime fechaNacimiento {get; set;}
        public string? foto {get; set;}
        public List<PeliculasActores>? peliculasActores {get; set;}
    }
}