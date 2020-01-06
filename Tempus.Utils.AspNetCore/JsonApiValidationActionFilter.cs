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
using Microsoft.Extensions.Logging;

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
        readonly ILogger _logger;

        public JsonApiValidationActionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<JsonApiValidationActionFilter>();
        }

        // Return a high number by default so that it runs closest to the action.
        public int Order { get; set; } = int.MaxValue - 10;
        
        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ExceptionHandled)
                return;

            context.ExceptionHandled =
                ProcessNotFoundException(context)
                || ProcessValidationException(context);
        }

        public virtual void OnActionExecuting(ActionExecutingContext context)
        {

        }

        bool ProcessValidationException(ActionExecutedContext context)
        {
            if (context.Exception != null && context.ModelState.TryAddModelError(context.Exception))
                _logger.LogError(context.Exception, "A validation exception occurred. It was returned to the client as a HTTP Status Code 400 fail.");

            var validationErrors = GetValidationMessagesFromModelState(context);

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
