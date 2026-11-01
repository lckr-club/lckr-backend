using System.ComponentModel.DataAnnotations;

namespace API_LCKR.Validations
{
    public class CpfValidoAttribute : ValidationAttribute
    {
        public CpfValidoAttribute()
        {
            ErrorMessage = "CPF inválido.";
        }

        public override bool IsValid(object? value)
        {
            if (value is not string cpf)
            {
                return false;
            }

            if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            {
                return false;
            }

            if (cpf.Distinct().Count() == 1)
            {
                return false;
            }

            return DigitoVerificador(cpf, 9) == cpf[9] - '0'
                && DigitoVerificador(cpf, 10) == cpf[10] - '0';
        }

        private static int DigitoVerificador(string cpf, int tamanho)
        {
            var soma = 0;

            for (var i = 0; i < tamanho; i++)
            {
                soma += (cpf[i] - '0') * (tamanho + 1 - i);
            }

            var resto = soma * 10 % 11;

            return resto == 10 ? 0 : resto;
        }
    }
}
