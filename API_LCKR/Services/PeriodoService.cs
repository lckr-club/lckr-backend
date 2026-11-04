using API_LCKR.DTOs;
using API_LCKR.Exceptions;
using API_LCKR.Helpers;
using API_LCKR.Mappers;
using API_LCKR.Models;
using API_LCKR.Repository;

namespace API_LCKR.Services
{
    public class PeriodoService
    {
        private readonly PeriodoRepository _repository;
        private readonly PagamentoRepository _pagamentoRepository;
        private readonly FolhaRepository _folhaRepository;
        private readonly FolhaService _folhaService;
        private readonly ExportacaoService _exportacaoService;
        private readonly EmailService _emailService;
        private readonly PeriodoMapper _mapper;
        private readonly PagamentoMapper _pagamentoMapper;

        public PeriodoService(
            PeriodoRepository repository,
            PagamentoRepository pagamentoRepository,
            FolhaRepository folhaRepository,
            FolhaService folhaService,
            ExportacaoService exportacaoService,
            EmailService emailService,
            PeriodoMapper mapper,
            PagamentoMapper pagamentoMapper)
        {
            _repository = repository;
            _pagamentoRepository = pagamentoRepository;
            _folhaRepository = folhaRepository;
            _folhaService = folhaService;
            _exportacaoService = exportacaoService;
            _emailService = emailService;
            _mapper = mapper;
            _pagamentoMapper = pagamentoMapper;
        }

        public List<PeriodoDto> ListarPeriodos()
        {
            return _repository.ListarPeriodos()
                .Select(_mapper.ParaDto)
                .ToList();
        }

        public PeriodoDto BuscarPeriodo(string mes)
        {
            var (inicio, fim) = MesReferencia.Intervalo(mes);
            var periodo = _repository.BuscarPorMes(mes);

            if (periodo != null)
            {
                return _mapper.ParaDto(periodo);
            }

            return new PeriodoDto
            {
                MesReferencia = mes,
                DataInicio = inicio,
                DataFim = fim,
                Status = StatusPeriodo.Aberto
            };
        }

        public FechamentoDto FecharPeriodo(string mes, bool confirmar)
        {
            var (inicio, fim) = MesReferencia.Intervalo(mes);
            var exibicao = MesReferencia.Exibicao(mes);
            var periodo = _repository.BuscarPorMes(mes);

            if (periodo?.Status == StatusPeriodo.Fechado)
            {
                throw new InvalidOperationException($"O período {exibicao} já está fechado.");
            }

            var agendadas = _folhaRepository.ContarAulasAgendadas(inicio, fim);

            if (agendadas > 0)
            {
                throw new InvalidOperationException($"O período {exibicao} tem {agendadas} aula(s) agendada(s). Marque como realizada ou exclua antes de fechar.");
            }

            var folha = _folhaService.CalcularFolha(mes);

            if (folha.Pendencias.Count > 0)
            {
                throw new InvalidOperationException($"Não é possível fechar o período {exibicao}. " + string.Join(" ", folha.Pendencias.Select(p => p.Mensagem)));
            }

            _pagamentoRepository.AtualizarAtrasados(DateOnly.FromDateTime(DateTime.Today));

            var pagamentosMes = _pagamentoRepository.ListarPagamentos(mes, null, null);
            var pendentes = pagamentosMes
                .Where(p => p.Status == StatusPagamento.Pendente || p.Status == StatusPagamento.Atrasado)
                .ToList();

            if (pendentes.Count > 0 && !confirmar)
            {
                throw new ConfirmacaoNecessariaException(
                    $"Os pagamentos abaixo estão pendentes e serão cancelados ao fechar o período {exibicao}, e novos pagamentos serão gerados. Confirme para fechar.",
                    pendentes.Select(_pagamentoMapper.ParaDto).ToList());
            }

            var cpfsPagos = pagamentosMes
                .Where(p => p.Status == StatusPagamento.Pago)
                .Select(p => p.CpfCoach)
                .ToHashSet();

            var organizacao = _folhaRepository.BuscarOrganizacao();
            var dataPagamento = MesReferencia.QuintoDiaUtil(inicio);
            var statusInicial = dataPagamento < DateOnly.FromDateTime(DateTime.Today) ? StatusPagamento.Atrasado : StatusPagamento.Pendente;

            var novos = folha.Coaches
                .Where(c => !cpfsPagos.Contains(c.CpfCoach))
                .Select(c => new Payment
                {
                    CpfCoach = c.CpfCoach,
                    Valor = c.Valor,
                    HorasTotais = c.Horas,
                    DataPagamento = dataPagamento,
                    Status = statusInicial,
                    MesReferencia = mes,
                    Descricao = $"Folha de pagamento {exibicao}",
                    OrganizationCnpj = organizacao.Cnpj
                })
                .ToList();

            var novo = periodo == null;

            periodo ??= new PayrollPeriod
            {
                MesReferencia = mes,
                DataInicio = inicio,
                DataFim = fim,
                OrganizationCnpj = organizacao.Cnpj
            };

            periodo.Status = StatusPeriodo.Fechado;
            periodo.DataFechamento = DateTime.UtcNow;
            periodo.UpdatedAt = DateTime.UtcNow;

            _repository.FecharPeriodo(periodo, novo, pendentes, novos);

            folha.StatusPeriodo = StatusPeriodo.Fechado;

            var emailEnviado = _emailService.EnviarFechamento(
                organizacao.Email,
                folha,
                dataPagamento,
                () => _exportacaoService.GerarPdf(folha, organizacao.Nome));

            return new FechamentoDto
            {
                Periodo = _mapper.ParaDto(periodo),
                Pagamentos = _pagamentoRepository.ListarPagamentos(mes, null, null)
                    .Where(p => p.Status != StatusPagamento.Cancelado)
                    .Select(_pagamentoMapper.ParaDto)
                    .ToList(),
                PagamentosCancelados = pendentes.Count,
                EmailEnviado = emailEnviado
            };
        }

        public PeriodoDto ReabrirPeriodo(string mes)
        {
            MesReferencia.Intervalo(mes);

            var exibicao = MesReferencia.Exibicao(mes);
            var periodo = _repository.BuscarPorMes(mes);

            if (periodo == null)
            {
                throw new KeyNotFoundException($"O período {exibicao} ainda não foi fechado.");
            }

            if (periodo.Status == StatusPeriodo.Aberto)
            {
                throw new InvalidOperationException($"O período {exibicao} já está aberto.");
            }

            if (_pagamentoRepository.ExistePagoNoMes(mes))
            {
                throw new InvalidOperationException($"O período {exibicao} tem pagamento já pago e não pode ser reaberto.");
            }

            periodo.Status = StatusPeriodo.Aberto;
            periodo.DataFechamento = null;

            _repository.AtualizarPeriodo(periodo);

            return _mapper.ParaDto(periodo);
        }
    }
}
