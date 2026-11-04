using API_LCKR.Models;

namespace API_LCKR.DTOs
{
    public class PeriodoDto
    {
        public Guid? Id { get; set; }

        public string MesReferencia { get; set; } = null!;

        public DateOnly DataInicio { get; set; }

        public DateOnly DataFim { get; set; }

        public StatusPeriodo Status { get; set; }

        public DateTime? DataFechamento { get; set; }
    }
}
