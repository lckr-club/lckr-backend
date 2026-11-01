using API_LCKR.Models;
using API_LCKR.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_LCKR.DTOs
{
    public class CoachDto
    {
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [CpfValido]
        public string Cpf { get; set; } = null!;

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail é inválido.")]
        [StringLength(150, ErrorMessage = "O e-mail deve ter no máximo 150 caracteres.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres.")]
        public string Telefone { get; set; } = null!;

        [Required(ErrorMessage = "A especialidade é obrigatória.")]
        [StringLength(100, ErrorMessage = "A especialidade deve ter no máximo 100 caracteres.")]
        public string Especialidade { get; set; } = null!;

        public StatusCoach Status { get; set; }

        [Range(typeof(decimal), "0.01", "99999999.99", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true, ErrorMessage = "O valor da hora deve ser maior que zero.")]
        [JsonPropertyName("valorHora")]
        public decimal SalarioAtual { get; set; }

        [Required(ErrorMessage = "O CNPJ da organização é obrigatório.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "O CNPJ deve ter 14 dígitos.")]
        public string OrganizationCnpj { get; set; } = null!;
    }
}
