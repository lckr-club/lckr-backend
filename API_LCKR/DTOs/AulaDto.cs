using System.ComponentModel.DataAnnotations;

namespace API_LCKR.DTOs
{
    public class AulaDto
    {
        [Required(ErrorMessage = "A data da aula é obrigatória.")]
        public DateOnly? DataAula { get; set; }

        [Required(ErrorMessage = "A hora de início é obrigatória.")]
        public TimeOnly? HoraInicio { get; set; }

        [Required(ErrorMessage = "A hora de fim é obrigatória.")]
        public TimeOnly? HoraFim { get; set; }

        [Required(ErrorMessage = "A modalidade é obrigatória.")]
        [StringLength(50, ErrorMessage = "A modalidade deve ter no máximo 50 caracteres.")]
        public string Modalidade { get; set; } = null!;

        [StringLength(255, ErrorMessage = "A descrição deve ter no máximo 255 caracteres.")]
        public string? Descricao { get; set; }

        [Range(1, 1000, ErrorMessage = "A capacidade deve ser entre 1 e 1000.")]
        public int Capacidade { get; set; }

        [Required(ErrorMessage = "O CPF do coach é obrigatório.")]
        public string CpfCoach { get; set; } = null!;
    }
}
