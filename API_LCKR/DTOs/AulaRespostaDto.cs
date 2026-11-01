using API_LCKR.Models;

namespace API_LCKR.DTOs
{
    public class AulaRespostaDto
    {
        public Guid Id { get; set; }

        public DateOnly DataAula { get; set; }

        public TimeOnly HoraInicio { get; set; }

        public TimeOnly HoraFim { get; set; }

        public string Modalidade { get; set; } = null!;

        public string? Descricao { get; set; }

        public int Capacidade { get; set; }

        public StatusLesson Status { get; set; }

        public string CpfCoach { get; set; } = null!;

        public string NomeCoach { get; set; } = null!;

        public string? CpfCoachPrevisto { get; set; }

        public string? NomeCoachPrevisto { get; set; }
    }
}
