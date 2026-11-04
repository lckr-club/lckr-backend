using API_LCKR.DTOs;
using API_LCKR.Helpers;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace API_LCKR.Services
{
    public class EmailService
    {
        private static readonly CultureInfo Cultura = new("pt-BR");

        private readonly IConfiguration _configuracao;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuracao, ILogger<EmailService> logger)
        {
            _configuracao = configuracao;
            _logger = logger;
        }

        public bool EnviarFechamento(string destino, FolhaDto folha, DateOnly dataPagamento, Func<byte[]> gerarPdf)
        {
            var secao = _configuracao.GetSection("Email");
            var host = secao["Host"];

            if (string.IsNullOrWhiteSpace(host))
            {
                _logger.LogWarning("Envio de e-mail não configurado. O aviso de fechamento da folha {Mes} não foi enviado.", folha.MesReferencia);
                return false;
            }

            try
            {
                var exibicao = MesReferencia.Exibicao(folha.MesReferencia);
                var remetente = string.IsNullOrWhiteSpace(secao["Remetente"]) ? secao["Usuario"] : secao["Remetente"];

                using var mensagem = new MailMessage(remetente!, destino)
                {
                    Subject = $"Folha de pagamento {exibicao} fechada",
                    Body = MontarCorpo(folha, exibicao, dataPagamento),
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8
                };

                mensagem.Attachments.Add(new Attachment(new MemoryStream(gerarPdf()), $"folha-{folha.MesReferencia}.pdf", "application/pdf"));

                using var cliente = new SmtpClient(host, secao.GetValue("Porta", 587))
                {
                    EnableSsl = secao.GetValue("UsarSsl", true)
                };

                if (!string.IsNullOrWhiteSpace(secao["Usuario"]))
                {
                    cliente.Credentials = new NetworkCredential(secao["Usuario"], secao["Senha"]);
                }

                cliente.Send(mensagem);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao enviar o e-mail de fechamento da folha {Mes}.", folha.MesReferencia);
                return false;
            }
        }

        private static string MontarCorpo(FolhaDto folha, string exibicao, DateOnly dataPagamento)
        {
            var corpo = new StringBuilder();

            corpo.AppendLine($"A folha de pagamento de {exibicao} foi fechada em {DateTime.Now:dd/MM/yyyy HH:mm}.");
            corpo.AppendLine();
            corpo.AppendLine($"Total de horas: {folha.TotalHoras.ToString("N2", Cultura)}");
            corpo.AppendLine($"Valor total: {folha.ValorTotal.ToString("C", Cultura)}");
            corpo.AppendLine($"Pagamento previsto para {dataPagamento:dd/MM/yyyy}.");
            corpo.AppendLine();

            foreach (var coach in folha.Coaches)
            {
                corpo.AppendLine($"{coach.NomeCoach}: {coach.Horas.ToString("N2", Cultura)} h, {coach.Valor.ToString("C", Cultura)}");
            }

            corpo.AppendLine();
            corpo.AppendLine("O relatório completo está em anexo.");

            return corpo.ToString();
        }
    }
}
