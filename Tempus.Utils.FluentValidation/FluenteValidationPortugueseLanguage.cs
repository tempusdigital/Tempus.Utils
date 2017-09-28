using FluentValidation.Validators;
using FluentValidation.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tempus.Utils.FluentValidation
{
    public class FluenteValidationPortugueseLanguage : LanguageManager
    {
        public FluenteValidationPortugueseLanguage()
        {
            AddTranslation("pt", nameof(EmailValidator), "Endereço de email inválido");
            AddTranslation("pt", nameof(GreaterThanOrEqualValidator), "Deve ser superior ou igual a '{ComparisonValue}'");
            AddTranslation("pt", nameof(GreaterThanValidator), "Deve ser superior a '{ComparisonValue}'");
            AddTranslation("pt", nameof(LengthValidator), "Deve ter entre {MinLength} e {MaxLength} caracteres");
            AddTranslation("pt", nameof(MinimumLengthValidator), "Deve ter ao menos {MinLength} caracteres");
            AddTranslation("pt", nameof(MaximumLengthValidator), "Deve ter no máximo {MaxLength} caracteres");
            AddTranslation("pt", nameof(LessThanOrEqualValidator), "Deve ser inferior a '{ComparisonValue}'");
            AddTranslation("pt", nameof(LessThanValidator), "Deve ser inferior ou igual a '{ComparisonValue}'");
            AddTranslation("pt", nameof(NotEmptyValidator), "Campo obrigatório");
            AddTranslation("pt", nameof(NotEqualValidator), "Deve ser diferente de '{ComparisonValue}'");
            AddTranslation("pt", nameof(NotNullValidator), "Campo obrigatório");
            AddTranslation("pt", nameof(PredicateValidator), "Valor inválido");
            AddTranslation("pt", nameof(AsyncPredicateValidator), "Valor inválido");
            AddTranslation("pt", nameof(RegularExpressionValidator), "Formato inválido");
            AddTranslation("pt", nameof(EqualValidator), "Deve ser igual a '{ComparisonValue}'");
            AddTranslation("pt", nameof(ExactLengthValidator), "Deve ter {MaxLength} caracteres, informados {TotalLength} caracteres");
            AddTranslation("pt", nameof(ExclusiveBetweenValidator), "Deve estar entre {From} e {To} (exclusivo)");
            AddTranslation("pt", nameof(InclusiveBetweenValidator), "Deve estar entre {From} e {To}");
        }
    }
}
