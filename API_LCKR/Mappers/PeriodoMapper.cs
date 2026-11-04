using API_LCKR.DTOs;
using API_LCKR.Models;

namespace API_LCKR.Mappers
{
    public class PeriodoMapper
    {
        public PeriodoDto ParaDto(PayrollPeriod periodo)
        {
            return new PeriodoDto
            {
                Id = periodo.Id,
                MesReferencia = periodo.MesReferencia,
                DataInicio = periodo.DataInicio,
                DataFim = periodo.DataFim,
                Status = periodo.Status,
                DataFechamento = periodo.DataFechamento
            };
        }
    }
}
