using API_LCKR.Models;

namespace API_LCKR.DTOs
{
    public class PagamentoDto
    {
        public Guid Id { get; set; }

        public string CpfCoach { get; set; } = null!;

        public string NomeCoach { get; set; } = null!;

        public string MesReferencia { get; set; } = null!;

        public decimal HorasTotais { get; set; }

        public decimal Valor { get; set; }

        public DateOnly DataPagamento { get; set; }

        public StatusPagamento Status { get; set; }

        public DateOnly? PagoEm { get; set; }

        public string Descricao { get; set; } = null!;
    }
}
