namespace API_LCKR.DTOs
{
    public class ResumoMesDto
    {
        public string MesReferencia { get; set; } = null!;

        public decimal TotalHoras { get; set; }

        public decimal HorasAgendadas { get; set; }

        public int CoachesAtivos { get; set; }

        public decimal ValorRealizado { get; set; }

        public decimal FolhaPrevista { get; set; }

        public int AulasSemValor { get; set; }
    }
}
