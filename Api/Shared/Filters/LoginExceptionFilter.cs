using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Shared.Filters
{
    public class LoginExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<LoginExceptionFilter> _logger;

        public LoginExceptionFilter(ILogger<LoginExceptionFilter> logger)
        {
            _logger = logger;
            
        }
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unhandled exception occurred during the request.");

            var response = new
            {
                IsAuthenticated = false,
                Message = context.Exception.Message,
                ErrorType = context.Exception.GetType().Name
            };

            if (context.Exception is UnauthorizedAccessException)
            {
                context.Result = new ObjectResult(response)
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }
            else if (context.Exception is PlatformNotSupportedException)
            {
                context.Result = new ObjectResult(response)
                {
                    StatusCode = (int)HttpStatusCode.NotImplemented
                };
            }
            else
            {
                context.Result = new ObjectResult(response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

        context.ExceptionHandled = true;
            
        }
    }
}