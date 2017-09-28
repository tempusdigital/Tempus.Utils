namespace Tempus.Utils.AspNetCore
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ModelBindingMessageProviderPortuguese
    {
        public static void Translate(this DefaultModelBindingMessageProvider source)
        {
            source.SetValueIsInvalidAccessor(
                (x) => "Valor inválido");
            source.SetValueMustBeANumberAccessor(
                (x) => "Informe somente números");
            source.SetMissingBindRequiredValueAccessor(
                (x) => "Campo obrigatório");
            source.SetAttemptedValueIsInvalidAccessor(
                (x, y) => $"O valor '{x}' não é válido para {y}.");
            source.SetMissingKeyOrValueAccessor(
                () => "Campo obrigatório");
            source.SetUnknownValueIsInvalidAccessor(
                (x) => "Valor inválido");
            source.SetValueMustNotBeNullAccessor(
                (x) => "Campo obrigatório");
            source.SetMissingRequestBodyRequiredValueAccessor(
                () => "Campo obrigatório");
        }
    }
}
