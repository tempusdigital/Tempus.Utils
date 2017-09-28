namespace Tempus.Utils.AspNetCore
{
    using global::FluentValidation;
    using global::FluentValidation.AspNetCore;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Tempus.Utils.FluentValidation;

    public static class FluentValidationMvcConfigurationExtensions
    {
        public static FluentValidationMvcConfiguration Translate(this FluentValidationMvcConfiguration source)
        {
            ValidatorOptions.LanguageManager = new FluenteValidationPortugueseLanguage();

            return source;
        }
    }
}
