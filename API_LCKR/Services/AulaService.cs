using API_LCKR.DTOs;
using API_LCKR.Helpers;
using API_LCKR.Mappers;
using API_LCKR.Models;
using API_LCKR.Repository;

namespace API_LCKR.Services
{
    public class AulaService
    {
        private readonly AulaRepository _repository;
        private readonly CoachRepository _coachRepository;
        private readonly PeriodoRepository _periodoRepository;
        private readonly AulaMapper _mapper;

        public AulaService(AulaRepository repository, CoachRepository coachRepository, PeriodoRepository periodoRepository, AulaMapper mapper)
        {
            _repository = repository;
            _coachRepository = coachRepository;
            _periodoRepository = periodoRepository;
            _mapper = mapper;
        }

        public AulaRespostaDto CriarAula(AulaDto aula)
        {
            ValidarHorario(aula);
            ValidarPeriodoAberto(aula.DataAula!.Value);
            BuscarCoachAtivo(aula.CpfCoach);
            ValidarSobreposicao(aula, null);

            var entidade = _mapper.ParaEntidade(aula);

            _repository.AdicionarAula(entidade);

            return BuscarAula(entidade.Id);
        }

        public List<AulaRespostaDto> ListarAulas(DateOnly? inicio, DateOnly? fim, string? cpfCoach)
        {
            return _repository.ListarAulas(inicio, fim, cpfCoach)
                .Select(_mapper.ParaDto)
                .ToList();
        }

        public AulaRespostaDto BuscarAula(Guid id)
        {
            return _mapper.ParaDto(BuscarEntidade(id));
        }

        public AulaRespostaDto AtualizarAula(Guid id, AulaDto aula)
        {
            var entidade = BuscarEntidade(id);

            ValidarHorario(aula);
            ValidarPeriodoAberto(entidade.DataAula);
            ValidarPeriodoAberto(aula.DataAula!.Value);

            if (aula.CpfCoach != entidade.CoachCpf)
            {
                BuscarCoachAtivo(aula.CpfCoach);
            }

            ValidarSobreposicao(aula, id);

            _mapper.Atualizar(entidade, aula);

            if (entidade.CoachCpfPrevisto == entidade.CoachCpf)
            {
                entidade.CoachCpfPrevisto = null;
            }

            _repository.AtualizarAula(entidade);

            return BuscarAula(id);
        }

        public void ExcluirAula(Guid id)
        {
            var entidade = BuscarEntidade(id);

            ValidarPeriodoAberto(entidade.DataAula);

            _repository.ExcluirAula(entidade);
        }

        public AulaRespostaDto AlterarStatus(Guid id, StatusLesson status)
        {
            var entidade = BuscarEntidade(id);

            ValidarPeriodoAberto(entidade.DataAula);

            if (status == StatusLesson.Realizada && entidade.DataAula > DateOnly.FromDateTime(DateTime.Today))
            {
                throw new InvalidOperationException("Uma aula com data futura não pode ser marcada como realizada.");
            }

            entidade.Status = status;

            _repository.AtualizarAula(entidade);

            return BuscarAula(id);
        }

        public AulaRespostaDto SubstituirCoach(Guid id, SubstituicaoDto substituicao)
        {
            var entidade = BuscarEntidade(id);
            var cpfSubstituto = substituicao.CpfSubstituto;

            ValidarPeriodoAberto(entidade.DataAula);

            if (cpfSubstituto == entidade.CoachCpf)
            {
                throw new InvalidOperationException("O substituto deve ser diferente do coach atual da aula.");
            }

            BuscarCoachAtivo(cpfSubstituto);

            if (_repository.ExisteSobreposicao(cpfSubstituto, entidade.DataAula, entidade.HoraInicio, entidade.HoraFim, id))
            {
                throw new InvalidOperationException("O coach substituto já tem uma aula nesse horário.");
            }

            var previsto = entidade.CoachCpfPrevisto ?? entidade.CoachCpf;

            entidade.CoachCpf = cpfSubstituto;
            entidade.CoachCpfPrevisto = cpfSubstituto == previsto ? null : previsto;

            _repository.AtualizarAula(entidade);

            return BuscarAula(id);
        }

        private Lesson BuscarEntidade(Guid id)
        {
            var entidade = _repository.BuscarAula(id);

            if (entidade == null)
            {
                throw new KeyNotFoundException("Aula não foi encontrada!");
            }

            return entidade;
        }

        private void BuscarCoachAtivo(string cpf)
        {
            var coach = _coachRepository.FindCoachCpf(cpf);

            if (coach == null)
            {
                throw new KeyNotFoundException("Coach não foi encontrado!");
            }

            if (coach.Status == StatusCoach.Inativo)
            {
                throw new InvalidOperationException("Coach inativo não pode receber aulas.");
            }
        }

        private void ValidarPeriodoAberto(DateOnly data)
        {
            if (_periodoRepository.EstaFechado(data))
            {
                throw new InvalidOperationException($"O período {MesReferencia.Exibicao(MesReferencia.DaData(data))} está fechado. Reabra o período para alterar as aulas.");
            }
        }

        private static void ValidarHorario(AulaDto aula)
        {
            if (aula.HoraFim <= aula.HoraInicio)
            {
                throw new ArgumentException("A hora de fim deve ser maior que a hora de início.");
            }
        }

        private void ValidarSobreposicao(AulaDto aula, Guid? ignorarId)
        {
            if (_repository.ExisteSobreposicao(aula.CpfCoach, aula.DataAula!.Value, aula.HoraInicio!.Value, aula.HoraFim!.Value, ignorarId))
            {
                throw new InvalidOperationException("O coach já tem uma aula nesse horário.");
            }
        }
    }
}
