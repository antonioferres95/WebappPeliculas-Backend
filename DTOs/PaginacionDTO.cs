namespace backend.DTOs
{
    public class PaginacionDTO
    {
        public int pagina {get; set;} = 1;
        public int recordsPorPagina = 10;
        private readonly int cantidadMaximaRecordsPorPagina = 50;

        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }
            set
            {
                /*Si el value recibido es mayor que la cant maxima, seteo los recordsporpag con cant maxima,
                caso contrario si es menor, entonces seteo recordsporpag con el value recibido*/
                recordsPorPagina = (value > cantidadMaximaRecordsPorPagina) ? cantidadMaximaRecordsPorPagina : value;
            }
        }

    }
}