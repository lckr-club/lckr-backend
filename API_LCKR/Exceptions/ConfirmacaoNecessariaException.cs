using API_LCKR.DTOs;

namespace API_LCKR.Exceptions
{
    public class ConfirmacaoNecessariaException : Exception
    {
        public List<PagamentoDto> PagamentosPendentes { get; }

        public ConfirmacaoNecessariaException(string mensagem, List<PagamentoDto> pagamentosPendentes)
            : base(mensagem)
        {
            PagamentosPendentes = pagamentosPendentes;
        }
    }
}
