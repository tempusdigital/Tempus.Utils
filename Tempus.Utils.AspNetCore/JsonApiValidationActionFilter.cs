using Microsoft.AspNetCore.Mvc;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Newtonsoft.Json;
using System.Data.SqlClient;    // ToDo: adicionar testes unitários
using System;
using System.Text;
using System.Net;
using Tempus.Utils;

namespace Tempus.Utils.AspNetCore
{
    // ToDo: tratar erros de permissão
    // ToDo: tratar erros de autenticação
    /// <summary>
    /// Captura os erros de validação do ModelState ou da exceção e completa a requisição retornando as mensagens de validação como JSON.
    /// Se estiver em debug também retorna as mensagens das exceções não tratadas.
    /// Fonte: <seealso cref="https://goo.gl/pOB0ka"/>
    /// </summary>
    public class JsonApiValidationActionFilter : IActionFilter, IOrderedFilter
    {
        // Return a high number by default so that it runs closest to the action.
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null && !context.ExceptionHandled)
                return;

            context.ExceptionHandled =
                ProcessNotFoundException(context)
                || ProcessValidationException(context)
                || ProcessInternalServerError(context);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        bool ProcessValidationException(ActionExecutedContext context)
        {
            var validationErrors = new List<ErrorViewModel>();

            validationErrors.AddRange(GetMessagesFromErpExpection(context));
            validationErrors.AddRange(GetMessagesFromValidationExpection(context));
            validationErrors.AddRange(GetMessagesFromDeleteExpection(context));
            validationErrors.AddRange(GetMessagesFromConcurrencyExpection(context));
            validationErrors.AddRange(GetValidationMessagesFromModelState(context));

            if (validationErrors.Any())
            {
                var errors = validationErrors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e));

                var result = new ErrorResultViewModel(errors);

                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(result);

                return true;
            }

            return false;
        }

        IEnumerable<ErrorViewModel> GetValidationMessagesFromModelState(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid && context.ModelState.ErrorCount > 0)
                foreach (var field in context.ModelState)
                {
                    if (field.Value.ValidationState == ModelValidationState.Invalid)
                        foreach (var error in field.Value.Errors)
                        {
                            var message = string.IsNullOrWhiteSpace(error.ErrorMessage) ? Recurso.predicate_error : error.ErrorMessage;
                            yield return new ErrorViewModel(field.Key, "custom", message);
                        }
                }
        }

        bool ProcessInternalServerError(ActionExecutedContext context)
        {
            if (context.HttpContext.Request.Headers.ContainsKey(CorsConstants.Origin))
            {
#if DEBUG
                context.HttpContext.Response.StatusCode = 500;
                context.Result = new JsonResult(new
                {
                    Message = GetExceptionsMessages(context.Exception),
                    Stack = GetExceptionsStackTraces(context.Exception)
                });
#else
                context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
#endif
                return true;
            }

            return false;
        }

        string GetExceptionsMessages(Exception exception)
        {
            var messages = new StringBuilder();

            messages.Append("SERVER ERROR: ");

            while (exception != null)
            {
                if (!string.IsNullOrWhiteSpace(exception.Message))
                {
                    messages.Append(exception.Message);

                    if (exception.InnerException != null)
                        messages.AppendLine("--- ");
                }

                exception = exception.InnerException;
            }

            return messages.ToString();
        }

        string GetExceptionsStackTraces(Exception exception)
        {
            var messages = new StringBuilder();

            while (exception != null)
            {
                messages.AppendLine(exception.GetType().FullName);
                messages.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return messages.ToString();
        }

        bool ProcessNotFoundException(ActionExecutedContext context)
        {
            if (context.Result is NotFoundResult)
                return true;

            if (context.Exception is NotFoundException)
            {
                context.Result = new NotFoundResult();

                return true;
            }

            return false;
        }

        IEnumerable<ErrorViewModel> GetMessagesFromErpExpection(ActionExecutedContext context)
        {
            if (context.Exception is ServerSideValidationException serverSide)
            {
                yield return new ErrorViewModel(serverSide.Message);
            }
        }

        IEnumerable<ErrorViewModel> GetMessagesFromValidationExpection(ActionExecutedContext context)
        {
            if (context.Exception is ValidationException)
            {
                var exception = context.Exception as ValidationException;
                foreach (var error in exception.Errors)
                {
                    var message = string.IsNullOrWhiteSpace(error.ErrorMessage) ? Recurso.predicate_error : error.ErrorMessage;

                    if (string.IsNullOrWhiteSpace(error.ErrorCode))
                        yield return new ErrorViewModel(error.PropertyName, "custom", message);
                    else
                        yield return new ErrorViewModel(error.PropertyName, error.ErrorCode, message);
                }
            }
        }

        IEnumerable<ErrorViewModel> GetMessagesFromConcurrencyExpection(ActionExecutedContext context)
        {
            if (context.Exception?.GetType()?.Name == "DbUpdateConcurrencyException" || context.Exception?.InnerException?.GetType()?.Name == "DbUpdateConcurrencyException")
            {
                yield return new ErrorViewModel("", "", Recurso.ErroDeConcorrencia);
            }
        }

        IEnumerable<ErrorViewModel> GetMessagesFromDeleteExpection(ActionExecutedContext context)
        {
            if (context.Exception?.InnerException is SqlException)
            {
                var exception = context.Exception.InnerException as SqlException;
                if (exception.Number == 547 && exception.Message.Contains("DELETE"))
                    yield return new ErrorViewModel("", "", Recurso.ErroAoExcluir);
            }
        }

        private class ErrorResultViewModel
        {
            public ErrorResultViewModel(IDictionary<string, IEnumerable<ErrorViewModel>> errors)
            {
                Errors = errors;
            }

            public IDictionary<string, IEnumerable<ErrorViewModel>> Errors { get; }
        }

        private class ErrorViewModel
        {
            public ErrorViewModel(string text) : this("", "", text)
            {

            }

            public ErrorViewModel(string propertyName, string type, string text)
            {
                if (string.IsNullOrWhiteSpace(propertyName))
                    PropertyName = "servidor";
                else
                    PropertyName = propertyName;

                if (string.IsNullOrWhiteSpace(type))
                    Type = "invalido";
                else
                    Type = type;

                Text = text;
            }

            [JsonIgnore]
            public string PropertyName { get; }

            public string Type { get; }

            public string Text { get; }
        }
    }
}
