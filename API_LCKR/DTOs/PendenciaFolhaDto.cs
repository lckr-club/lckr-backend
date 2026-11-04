namespace API_LCKR.DTOs
{
    public class PendenciaFolhaDto
    {
        public string CpfCoach { get; set; } = null!;

        public string NomeCoach { get; set; } = null!;

        public Guid IdAula { get; set; }

        public DateOnly DataAula { get; set; }

        public TimeOnly HoraInicio { get; set; }

        public string Mensagem { get; set; } = null!;
    }
}
