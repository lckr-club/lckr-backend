namespace API_LCKR.DTOs
{
    public class FolhaAulaDto
    {
        public Guid IdAula { get; set; }

        public DateOnly DataAula { get; set; }

        public TimeOnly HoraInicio { get; set; }

        public TimeOnly HoraFim { get; set; }

        public string Modalidade { get; set; } = null!;

        public decimal Horas { get; set; }

        public decimal ValorHora { get; set; }

        public decimal Valor { get; set; }

        public string? CpfCoachPrevisto { get; set; }
    }
}
