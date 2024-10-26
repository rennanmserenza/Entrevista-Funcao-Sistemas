using System.Text.RegularExpressions;

namespace FI.AtividadeEntrevista.Extensions
{
    public static class StringExtension
    {
        public static bool ValidarCPF(string cpf)
        {
            // Remove caracteres não numéricos
            cpf = Regex.Replace(cpf, @"[^\d]", "");

            // Verifica se o CPF tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais (ex.: "111.111.111-11")
            if (new string(cpf[0], cpf.Length) == cpf)
                return false;

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (cpf[i] - '0') * (10 - i);

            int primeiroDigitoVerificador = (soma * 10) % 11;
            if (primeiroDigitoVerificador == 10)
                primeiroDigitoVerificador = 0;

            // Verifica o primeiro dígito
            if (primeiroDigitoVerificador != (cpf[9] - '0'))
                return false;

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (cpf[i] - '0') * (11 - i);

            int segundoDigitoVerificador = (soma * 10) % 11;
            if (segundoDigitoVerificador == 10)
                segundoDigitoVerificador = 0;

            // Verifica o segundo dígito
            if (segundoDigitoVerificador != (cpf[10] - '0'))
                return false;

            return true;
        }
    }
}
