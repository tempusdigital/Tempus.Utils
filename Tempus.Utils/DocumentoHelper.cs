namespace Tempus.Utils
{
    using System;

    public static class DocumentoHelper
    {
        /// <summary>
        /// Verifica se um cnpj é válido e formata com a máscara correta. Ignora a máscara pré-existente na validação.
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="cpfFormatado">Retorna CPF formatado caso seja válido</param>
        /// <returns>Retorna true caso seja um CPF válido</returns>
        public static bool TryParseCpf(string cpf, out string cpfFormatado)
        {
            cpfFormatado = string.Empty;

            if (!string.IsNullOrWhiteSpace(cpf))
            {
                cpf = cpf.Replace(".", "").Replace("-", "").Trim();

                if (IsCpf(cpf))
                {
                    cpfFormatado = cpf.FormatWithMask("###.###.###-##");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Valida se um CPF é válido.
        /// Fonte: http://www.macoratti.net/11/09/c_val1.htm
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool IsCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        /// <summary>
        /// Converte uma string em um CNPJ com a máscara. Ignora a máscara pré-existente na validação.
        /// </summary>
        /// <param name="cnpj"></param>
        /// <param name="cnpjFormatado">Retorna CNPJ formatado caso entrada seja válida</param>
        /// <returns>Retorna true caso seja um CNPJ válido</returns>
        public static bool TryParseCnpj(string cnpj, out string cnpjFormatado)
        {
            cnpjFormatado = string.Empty;

            if (!string.IsNullOrWhiteSpace(cnpj))
            {
                cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Trim();

                if (IsCnpj(cnpj))
                {
                    cnpjFormatado = cnpj.FormatWithMask("##.###.###/####-##");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verifica se um CNPJ é válido.
        /// Fonte: http://www.macoratti.net/11/09/c_val1.htm
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public static bool IsCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }
    }
}