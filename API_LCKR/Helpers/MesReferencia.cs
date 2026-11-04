using System.Globalization;
using System.Text.RegularExpressions;

namespace API_LCKR.Helpers
{
    public static class MesReferencia
    {
        private static readonly Regex Formato = new(@"^\d{4}-(0[1-9]|1[0-2])$");

        public static (DateOnly Inicio, DateOnly Fim) Intervalo(string? mes)
        {
            if (string.IsNullOrWhiteSpace(mes) || !Formato.IsMatch(mes))
            {
                throw new ArgumentException("Mês de referência inválido. Use o formato AAAA-MM, por exemplo 2026-10.");
            }

            var inicio = DateOnly.ParseExact(mes + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return (inicio, inicio.AddMonths(1).AddDays(-1));
        }

        public static string DaData(DateOnly data)
        {
            return data.ToString("yyyy-MM", CultureInfo.InvariantCulture);
        }

        public static string Exibicao(string mes)
        {
            return $"{mes.Substring(5, 2)}/{mes.Substring(0, 4)}";
        }

        public static DateOnly QuintoDiaUtil(DateOnly inicioMes)
        {
            var dia = inicioMes.AddMonths(1);
            var diasUteis = 0;

            while (true)
            {
                if (dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasUteis++;

                    if (diasUteis == 5)
                    {
                        return dia;
                    }
                }

                dia = dia.AddDays(1);
            }
        }
    }
}
