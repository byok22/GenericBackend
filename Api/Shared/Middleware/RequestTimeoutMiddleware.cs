using System.Diagnostics;

namespace Shared.Middleware
{
    public class RequestTimeoutMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimeoutMiddleware> _logger;
        private readonly TimeSpan _timeout;

        public RequestTimeoutMiddleware(RequestDelegate next, ILogger<RequestTimeoutMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _timeout = TimeSpan.FromMinutes(2); 
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Options)
            {
                await _next(context);
                return;
            }

            // Creamos un token que combine el token del usuario (si cancela en navegador) + nuestro timeout
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted);
            timeoutCts.CancelAfter(_timeout);
            
            // Reemplazamos el token del contexto para que los controladores sepan que deben cancelar
            context.RequestAborted = timeoutCts.Token;

            try
            {
                // Ejecutamos el siguiente paso. 
                // NOTA: Si el controlador respeta 'cancellationToken', se detendrá solo al vencer el tiempo.
                await _next(context);
            }
            catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested)
            {
                // Entra aquí si el timeout se disparó O si el usuario canceló
                _logger.LogWarning($"Request timed out after {_timeout.TotalMinutes} minutes: {context.Request.Path}");
                
                // IMPORTANTE: Verificar si la respuesta ya empezó a enviarse para evitar CRASH
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status504GatewayTimeout; // 504 es más correcto para timeout
                    await context.Response.WriteAsync("Request timed out.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred: {context.Request.Path}");
                throw; // Deja que tu ExceptionFilter o middleware de errores global lo maneje
            }
        }
    }

    public static class RequestTimeoutMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestTimeout(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTimeoutMiddleware>();
        }
    }
}