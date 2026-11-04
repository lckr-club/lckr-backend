namespace API_LCKR.DTOs
{
    public class FolhaCoachDto
    {
        public string CpfCoach { get; set; } = null!;

        public string NomeCoach { get; set; } = null!;

        public int QuantidadeAulas { get; set; }

        public decimal Horas { get; set; }

        public decimal Valor { get; set; }

        public List<FolhaAulaDto> Aulas { get; set; } = new();
    }
}
