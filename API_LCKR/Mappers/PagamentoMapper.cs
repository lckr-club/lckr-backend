using API_LCKR.DTOs;
using API_LCKR.Models;

namespace API_LCKR.Mappers
{
    public class PagamentoMapper
    {
        public PagamentoDto ParaDto(Payment pagamento)
        {
            return new PagamentoDto
            {
                Id = pagamento.Id,
                CpfCoach = pagamento.CpfCoach,
                NomeCoach = pagamento.CpfCoachNavigation?.Nome ?? string.Empty,
                MesReferencia = pagamento.MesReferencia,
                HorasTotais = pagamento.HorasTotais,
                Valor = pagamento.Valor,
                DataPagamento = pagamento.DataPagamento,
                Status = pagamento.Status,
                PagoEm = pagamento.PagoEm,
                Descricao = pagamento.Descricao
            };
        }
    }
}
