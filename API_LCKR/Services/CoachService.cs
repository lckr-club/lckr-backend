using API_LCKR.DTOs;
using API_LCKR.Mappers;
using API_LCKR.Models;
using API_LCKR.Repository;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API_LCKR.Services
{
    public class CoachService
    {
        private readonly CoachRepository _repository;
        private readonly CoachMapper _mapper;

        public CoachService(CoachRepository repository, CoachMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public CoachDto CreateCoach(CoachDto coach)
        {
            var findCoach = _repository.FindCoachDto(coach);
            var findEmail = _repository.FindCoachEmail(coach.Email);

            if (findCoach != null)
            {
                throw new InvalidOperationException("Coach já está cadastrado!");
            }

            if (findEmail != null)
            {
                throw new InvalidOperationException("Email já está cadastrado!");
            }

            var entidade = _mapper.ParaEntidade(coach);

            entidade.CoachSalaryHistories.Add(new CoachSalaryHistory
            {
                Salario = entidade.SalarioAtual,
                DataInicio = entidade.DataAdmissao,
                Motivo = "Valor inicial"
            });

            _repository.addCoach(entidade);

            return _mapper.ParaDto(entidade);
        }

        public List<CoachDto?> ListCoaches()
        {
            var coaches = _repository.ListCoaches();

            List<CoachDto> list = new List<CoachDto>();

            foreach (Coach coach in coaches)
            {
                var forDto = _mapper.ParaDto(coach);

                list.Add(forDto);
            }

            return list;
        }

        public CoachDto UpdateCoach(string cpf, CoachDto coach)
        {
            var findCoach = _repository.FindCoachCpf(cpf);
            var findEmail = _repository.FindCoachEmail(coach.Email);

            if (findCoach == null)
            {
                throw new KeyNotFoundException("Cpf não foi encontrado!");
            }

            if (findEmail != null && findEmail.Cpf != cpf)
            {
                throw new InvalidOperationException("Email já está cadastrado!");
            }

            _repository.UpdateCoach(findCoach, coach);

            return _mapper.ParaDto(findCoach);
        }

        public CoachDto GetCoach(string cpf)
        {
            var findCoach = _repository.FindCoachCpf(cpf);

            if (findCoach == null)
            {
                throw new KeyNotFoundException("Cpf não foi encontrado!");
            }
            
            return _mapper.ParaDto(findCoach);
        }

        public CoachDto InativarCoach(string cpf)
        {
            var findCoach = _repository.FindCoachCpf(cpf);
            bool pendencias = _repository.Pendencias(cpf);

            if (findCoach == null)
            {
                throw new KeyNotFoundException("Cpf não foi encontrado!");
            }

            if (pendencias)
            {
                throw new InvalidOperationException("Coach possui aulas agendadas ou pagamentos pendentes e não pode ser inativado.");
            }

            _repository.InativarCoach(findCoach);

            return _mapper.ParaDto(findCoach);
        }

        public CoachDto ReativarCoach(string cpf)
        {
            var findCoach = _repository.FindCoachCpf(cpf);

            if (findCoach == null)
            {
                throw new KeyNotFoundException("Cpf não foi encontrado!");
            }

            if (findCoach.Status == StatusCoach.Ativo)
            {
                throw new InvalidOperationException("Coach já está ativo!");
            }

            _repository.ReativarCoach(findCoach);

            return _mapper.ParaDto(findCoach);
        }
    }
}
