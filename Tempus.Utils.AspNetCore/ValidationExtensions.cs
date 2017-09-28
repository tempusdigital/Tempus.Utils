using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tempus.Utils;

namespace Tempus.Utils.FluentValidation
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, IFormFile> Formato<T>(this IRuleBuilder<T, IFormFile> ruleBuilder, string formatosValidos)
        {
            var fileFormats = formatosValidos.Split(',', ';', '.').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            return Formato(ruleBuilder, fileFormats);
        }

        public static IRuleBuilderOptions<T, IFormFile> Formato<T>(this IRuleBuilder<T, IFormFile> ruleBuilder, string[] formatosValidos)
        {
            return ruleBuilder
                .Must(file =>
                {
                    if (file == null)
                        return true;

                    var format = file.FileName?.Split('.')?.Last()?.ToLower();

                    return !string.IsNullOrEmpty(format) && formatosValidos.Contains(format);
                })
                .WithMessage("O arquivo deve ser do formato: " + string.Join(", ", formatosValidos));
        }

        public static bool AdicionarErrosDeValidacao(this ModelStateDictionary modelState, Exception ex)
        {
            return
                GetMessagesFromServerValidationExpection(modelState, ex)
                || GetMessagesFromValidationExpection(modelState, ex);
            //GetMessagesFromDeleteExpection(modelState, ex);
            //GetMessagesFromConcurrencyExpection(modelState, ex);
        }

        static bool GetMessagesFromServerValidationExpection(this ModelStateDictionary modelState, Exception ex)
        {
            if (ex is ServerSideValidationException serverSide)
            {
                modelState.AddModelError("", serverSide.Message);
                return true;
            }

            return false;
        }

        static bool GetMessagesFromValidationExpection(this ModelStateDictionary modelState, Exception ex)
        {
            if (ex is ValidationException exception)
            {
                foreach (var error in exception.Errors)
                {
                    var message = string.IsNullOrWhiteSpace(error.ErrorMessage) ? "Valor inválido" : error.ErrorMessage;
                    modelState.AddModelError(error.PropertyName, message);
                }

                return true;
            }

            return false;
        }
    }
}
