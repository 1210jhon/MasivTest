using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rest.API.Infrastructure.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Rest.API.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        #region Variables

        private readonly IHostEnvironment env;
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        #endregion

        #region Builders

        public HttpGlobalExceptionFilter(IHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        #endregion

        #region Methods

        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            if (context.Exception.GetType() == typeof(RouletteDomainException))
            {
                JsonErrorResponse json = null;
                var innerException = context.Exception.InnerException;

                if (innerException == null)
                {
                    json = new JsonErrorResponse
                    {
                        Messages = new string[] { context.Exception.Message }
                    };
                }
                else
                {
                    var errors = ((FluentValidation.ValidationException)innerException).Errors.Select(x => x.ErrorMessage).ToArray();
                    json = new JsonErrorResponse
                    {
                        Messages = errors
                    };
                }
                context.Result = new BadRequestObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception.GetType() == typeof(KeyNotFoundException))
            {
                var json = new JsonErrorResponse
                {
                    Messages = new[] { "El objeto solicitado no se encuentra registrado" }
                };

                if (env.IsDevelopment())
                {
                    json.DeveloperMessage = context.Exception;
                }

                context.Result = new KeyNotFoundObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                var json = new JsonErrorResponse
                {
                    Messages = new[] { "Ocurrió un error interno, intente nuevamente por favor.", context.Exception.Message }
                };

                if (env.IsDevelopment())
                {
                    json.DeveloperMessage = context.Exception;
                }

                context.Result = new InternalServerErrorObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
        }
        private class JsonErrorResponse
        {
            public string[] Messages { get; set; }
            public object DeveloperMessage { get; set; }
        }

        #endregion
    }

    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error) : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
    public class KeyNotFoundObjectResult : ObjectResult
    {
        public KeyNotFoundObjectResult(object error) : base(error)
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
    }
}
