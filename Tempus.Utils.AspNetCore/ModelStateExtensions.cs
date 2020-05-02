using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Tempus.Utils.AspNetCore
{
    public static class ModelStateExtensions
    {
        public static bool TryAddModelError(this ModelStateDictionary source, Exception exception)
        {
            return
                TryProcessServerSideException(source, exception)
                || TryProcessValidationExpection(source, exception)
                || TryProcessDeleteExpection(source, exception)
                || TryProcessConcurrencyExpection(source, exception);
        }

        static bool TryProcessServerSideException(ModelStateDictionary source, Exception exception)
        {
            if (exception is ServerSideValidationException serverSide)
            {
                source.AddModelError("", serverSide.Message);
                return true;
            }

            return false;
        }

        static bool TryProcessValidationExpection(ModelStateDictionary source, Exception exception)
        {
            if (exception is ValidationException validationException)
            {
                foreach (var error in validationException.Errors)
                {
                    var message = string.IsNullOrWhiteSpace(error.ErrorMessage) ? Recurso.predicate_error : error.ErrorMessage;

                    if (string.IsNullOrWhiteSpace(error.ErrorCode))
                        source.AddModelError(error.PropertyName, message);
                    else
                        source.AddModelError(error.PropertyName, message);
                }

                return true;
            }

            return false;
        }

        static bool TryProcessConcurrencyExpection(ModelStateDictionary source, Exception exception)
        {
            if (exception?.GetType()?.Name == "DbUpdateConcurrencyException" || exception?.InnerException?.GetType()?.Name == "DbUpdateConcurrencyException")
            {
                source.AddModelError("", Recurso.ErroDeConcorrencia);
                return true;
            }

            return false;
        }

        static bool TryProcessDeleteExpection(ModelStateDictionary source, Exception exception)
        {
            if (IsSqlServerDeleteReferenceException(exception) || IsPostgreDeleteReferenceException(exception))
            {
                source.AddModelError("", Recurso.ErroAoExcluir);
                return true;
            }

            return false;
        }

        static bool IsSqlServerDeleteReferenceException(Exception exception)
        {
            return exception.Message != null
                && exception.Message.Contains("DELETE")
                && exception.Message.Contains("REFERENCE")
                && exception.Message.Contains("constraint")
                || (exception.InnerException != null && IsSqlServerDeleteReferenceException(exception.InnerException));
        }

        static bool IsPostgreDeleteReferenceException(Exception exception)
        {
            return exception.GetType().Name == "PostgresException"
                && exception.Message.Contains("violates foreign key constraint")
                || (exception.InnerException != null && IsPostgreDeleteReferenceException(exception.InnerException));
        }
    }
}
