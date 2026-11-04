namespace API_LCKR.DTOs
{
    public class AvisoFechamentoDto
    {
        public string Mensagem { get; set; } = null!;

        public bool RequerConfirmacao { get; set; } = true;

        public List<PagamentoDto> PagamentosPendentes { get; set; } = new();
    }
}
