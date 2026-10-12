using API_LCKR.DTOs;
using API_LCKR.Models;

namespace API_LCKR.Mappers
{
    public class CoachMapper
    {
        public Coach ParaEntidade(CoachDto coach)
        {
            var addCoachDto = new Coach
            {
                Cpf = coach.Cpf,
                Nome = coach.Nome,
                Email = coach.Email,
                Senha = coach.Senha,
                Telefone = coach.Telefone,
                Especialidade = coach.Especialidade,
                Status = coach.Status,
                SalarioAtual = coach.SalarioAtual,
                OrganizationCnpj = coach.OrganizationCnpj,
            };
            return addCoachDto;
        }

        public CoachDto ParaDto(Coach coach)
        {
            var forDto = new CoachDto
            {
                Cpf = coach.Cpf,
                Nome = coach.Nome,
                Email = coach.Email,
                Senha = coach.Senha,
                Telefone = coach.Telefone,
                Especialidade = coach.Especialidade,
                Status = coach.Status,
                SalarioAtual = coach.SalarioAtual,
                OrganizationCnpj = coach.OrganizationCnpj,
            };
            return forDto;
        }
    }
}
