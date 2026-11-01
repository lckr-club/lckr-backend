using API_LCKR.DTOs;
using API_LCKR.Mappers;
using API_LCKR.Models;
using API_LCKR.Repository;

namespace API_LCKR.Services
{
    public class HistoricoValorService
    {
        private readonly HistoricoValorRepository _repository;
        private readonly CoachRepository _coachRepository;
        private readonly HistoricoValorMapper _mapper;

        public HistoricoValorService(HistoricoValorRepository repository, CoachRepository coachRepository, HistoricoValorMapper mapper)
        {
            _repository = repository;
            _coachRepository = coachRepository;
            _mapper = mapper;
        }

        public List<HistoricoValorDto> ListarHistorico(string cpf)
        {
            BuscarCoach(cpf);

            return _repository.ListarHistorico(cpf)
                .Select(_mapper.ParaDto)
                .ToList();
        }

        public HistoricoValorDto ValorVigente(string cpf, DateOnly data)
        {
            BuscarCoach(cpf);

            var vigente = _repository.BuscarVigente(cpf, data);

            if (vigente == null)
            {
                throw new KeyNotFoundException($"O coach não possui valor da hora vigente em {data:dd/MM/yyyy}.");
            }

            return _mapper.ParaDto(vigente);
        }

        public HistoricoValorDto RegistrarReajuste(string cpf, ReajusteDto reajuste)
        {
            var coach = BuscarCoach(cpf);
            var dataInicio = reajuste.DataInicio!.Value;
            var ultimo = _repository.BuscarUltimo(cpf);

            if (ultimo != null && dataInicio <= ultimo.DataInicio)
            {
                throw new InvalidOperationException($"A data de início deve ser posterior a {ultimo.DataInicio:dd/MM/yyyy}, início do valor atual.");
            }

            var novo = new CoachSalaryHistory
            {
                CoachCpf = cpf,
                Salario = reajuste.ValorHora,
                DataInicio = dataInicio,
                Motivo = reajuste.Motivo
            };

            var anterior = ultimo != null && (ultimo.DataFim == null || ultimo.DataFim >= dataInicio) ? ultimo : null;

            _repository.RegistrarReajuste(coach, anterior, novo);

            return _mapper.ParaDto(novo);
        }

        private Coach BuscarCoach(string cpf)
        {
            var coach = _coachRepository.FindCoachCpf(cpf);

            if (coach == null)
            {
                throw new KeyNotFoundException("Cpf não foi encontrado!");
            }

            return coach;
        }
    }
}
