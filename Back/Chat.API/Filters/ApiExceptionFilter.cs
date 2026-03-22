using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security;

namespace Chipis.API.Filters
{
    public class ApiExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;
            var traceId = context.HttpContext.TraceIdentifier;

            var (status, message) = MapException(exception);

            _logger.LogError(exception,
                "Unhandled exception occurred. TraceId: {TraceId}", traceId);

            context.Result = new ObjectResult(new
            {
                error = new
                {
                    message,
                    type = exception.GetType().Name,
                    traceId
                }
            })
            {
                StatusCode = (int)status
            };

            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }

        private static (HttpStatusCode status, string message) MapException(Exception ex)
        {
            return ex switch
            {
                InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),
                ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, ex.Message),
                KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
                SecurityException => (HttpStatusCode.Forbidden, ex.Message),

                _ => (HttpStatusCode.InternalServerError, "Unexpected server error")
            };
        }
    }
}
