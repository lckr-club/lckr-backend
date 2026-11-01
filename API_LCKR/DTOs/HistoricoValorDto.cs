namespace API_LCKR.DTOs
{
    public class HistoricoValorDto
    {
        public Guid Id { get; set; }

        public string CpfCoach { get; set; } = null!;

        public decimal ValorHora { get; set; }

        public DateOnly DataInicio { get; set; }

        public DateOnly? DataFim { get; set; }

        public string? Motivo { get; set; }
    }
}
