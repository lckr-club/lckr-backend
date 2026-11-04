using API_LCKR.Models;

namespace API_LCKR.DTOs
{
    public class FolhaDto
    {
        public string MesReferencia { get; set; } = null!;

        public DateOnly DataInicio { get; set; }

        public DateOnly DataFim { get; set; }

        public StatusPeriodo StatusPeriodo { get; set; }

        public decimal TotalHoras { get; set; }

        public decimal ValorTotal { get; set; }

        public List<FolhaCoachDto> Coaches { get; set; } = new();

        public List<PendenciaFolhaDto> Pendencias { get; set; } = new();
    }
}
