namespace API_LCKR.DTOs
{
    public class FechamentoDto
    {
        public PeriodoDto Periodo { get; set; } = null!;

        public List<PagamentoDto> Pagamentos { get; set; } = new();

        public int PagamentosCancelados { get; set; }

        public bool EmailEnviado { get; set; }
    }
}
