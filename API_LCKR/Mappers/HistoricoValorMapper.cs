using API_LCKR.DTOs;
using API_LCKR.Models;

namespace API_LCKR.Mappers
{
    public class HistoricoValorMapper
    {
        public HistoricoValorDto ParaDto(CoachSalaryHistory historico)
        {
            return new HistoricoValorDto
            {
                Id = historico.Id,
                CpfCoach = historico.CoachCpf,
                ValorHora = historico.Salario,
                DataInicio = historico.DataInicio,
                DataFim = historico.DataFim,
                Motivo = historico.Motivo
            };
        }
    }
}
