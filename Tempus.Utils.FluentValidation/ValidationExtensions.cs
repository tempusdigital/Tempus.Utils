using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tempus.Utils;

namespace Tempus.Utils.FluentValidation
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> SomenteLetrasComAcentosOuNumerosOuEspaco<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(ValidationConst.SomenteLetrasComAcentosOuNumerosOuEspaco)
                .WithMessage("Informe somente letras, números ou espaço");
        }

        public static IRuleBuilderOptions<T, string> SomenteLetrasComAcentosOuNumerosOuEspacoOuPonto<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(ValidationConst.SomenteLetrasComAcentosOuNumerosOuEspacoOuPonto)
                .WithMessage("Informe somente letras, ponto ou espaço");
        }

        public static IRuleBuilderOptions<T, string> SomenteNumerosOuLetrasSemAcentos<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(ValidationConst.SomenteNumerosOuLetrasSemAcentos)
                .WithMessage("Informe somente números ou letras");
        }

        public static IRuleBuilderOptions<T, string> SomenteNumeros<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(ValidationConst.SomenteNumeros)
                .WithMessage("Informe somente números");
        }

        public static IRuleBuilderInitial<T, string> ValidarCpfOuCnpj<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Custom((documento, context) =>
                {
                    if (string.IsNullOrEmpty(documento))
                        return;

                    if (DocumentoHelper.IsCnpj(documento))
                    {
                        if (!DocumentoHelper.TryParseCnpj(documento, out var cpnj))
                            context.AddFailure("CNPJ inválido");
                    }
                    else
                    {
                        if (!DocumentoHelper.TryParseCpf(documento, out var cpf))
                            context.AddFailure("CPF inválido");
                    }
                });
        }

        public static IRuleBuilderOptions<T, string> ValidarCnpj<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(documento =>
                     string.IsNullOrEmpty(documento) || DocumentoHelper.TryParseCnpj(documento, out var cnpj))
                .WithMessage("CNPJ inválido");
        }

        public static IRuleBuilderOptions<T, string> ValidarCpf<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(documento =>
                     string.IsNullOrEmpty(documento) || DocumentoHelper.TryParseCpf(documento, out var cpf))
                .WithMessage("CPF inválido");
        }
    }
}
