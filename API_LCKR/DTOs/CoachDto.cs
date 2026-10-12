using API_LCKR.Models;

namespace API_LCKR.DTOs
{
    public class CoachDto
    {
        public string Cpf { get; set; } = null!;

        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Senha { get; set; } = null!;

        public string Telefone { get; set; } = null!;

        public string Especialidade { get; set; } = null!;

        public StatusCoach Status { get; set; }

        public decimal SalarioAtual { get; set; }

        public string OrganizationCnpj { get; set; } = null!;

    }
}
