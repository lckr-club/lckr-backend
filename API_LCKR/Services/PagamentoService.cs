using API_LCKR.DTOs;
using API_LCKR.Helpers;
using API_LCKR.Mappers;
using API_LCKR.Models;
using API_LCKR.Repository;

namespace API_LCKR.Services
{
    public class PagamentoService
    {
        private readonly PagamentoRepository _repository;
        private readonly PagamentoMapper _mapper;

        public PagamentoService(PagamentoRepository repository, PagamentoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<PagamentoDto> ListarPagamentos(string? mes, string? cpfCoach, StatusPagamento? status)
        {
            if (!string.IsNullOrWhiteSpace(mes))
            {
                MesReferencia.Intervalo(mes);
            }

            _repository.AtualizarAtrasados(Hoje());

            return _repository.ListarPagamentos(mes, cpfCoach, status)
                .Select(_mapper.ParaDto)
                .ToList();
        }

        public PagamentoDto BuscarPagamento(Guid id)
        {
            _repository.AtualizarAtrasados(Hoje());

            return _mapper.ParaDto(BuscarEntidade(id));
        }

        public PagamentoDto MarcarPago(Guid id, DateOnly? pagoEm)
        {
            var data = pagoEm ?? Hoje();

            if (data > Hoje())
            {
                throw new ArgumentException("A data do pagamento não pode ser futura.");
            }

            _repository.AtualizarAtrasados(Hoje());

            var pagamento = BuscarEntidade(id);

            if (pagamento.Status == StatusPagamento.Pago)
            {
                throw new InvalidOperationException("Pagamento já está pago.");
            }

            if (pagamento.Status == StatusPagamento.Cancelado)
            {
                throw new InvalidOperationException("Pagamento cancelado não pode ser marcado como pago.");
            }

            pagamento.Status = StatusPagamento.Pago;
            pagamento.PagoEm = data;

            _repository.AtualizarPagamento(pagamento);

            return _mapper.ParaDto(pagamento);
        }

        private Payment BuscarEntidade(Guid id)
        {
            var pagamento = _repository.BuscarPagamento(id);

            if (pagamento == null)
            {
                throw new KeyNotFoundException("Pagamento não foi encontrado!");
            }

            return pagamento;
        }

        private static DateOnly Hoje()
        {
            return DateOnly.FromDateTime(DateTime.Today);
        }
    }
}
