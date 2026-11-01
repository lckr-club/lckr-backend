using System.ComponentModel.DataAnnotations;

namespace API_LCKR.DTOs
{
    public class ReajusteDto
    {
        [Range(typeof(decimal), "0.01", "99999999.99", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true, ErrorMessage = "O valor da hora deve ser maior que zero.")]
        public decimal ValorHora { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateOnly? DataInicio { get; set; }

        [StringLength(255, ErrorMessage = "O motivo deve ter no máximo 255 caracteres.")]
        public string? Motivo { get; set; }
    }
}
