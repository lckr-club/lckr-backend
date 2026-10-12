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

        public string CreateCoach(CoachDto coach)
        {
            var findCoach = _repository.FindCoachDto(coach);

            if (findCoach != null)
            {
                return "Coach já está cadastrado!";
            }

            _repository.addCoach(_mapper.ParaEntidade(coach));
            return "Coach adicionado!";
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

        public string UpdateCoach(string cpf, CoachDto coach)
        {
            var findCoach = _repository.FindCoachCpf(cpf);
            if (findCoach == null)
            {
                return "Cpf não foi encotrado";
            }

            _repository.UpdateCoach(findCoach, coach);
            return "Coach atualizado!";
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

        public CoachDto DeleteCoach(string cpf)
        {
            var findCoach = _repository.FindCoachCpf(cpf);

            if (findCoach == null)
            {
                throw new KeyNotFoundException("Cpf não foi encontrado!");
            }

            _repository.DeleteCoach(findCoach);

            return _mapper.ParaDto(findCoach);
        }
    }
}
