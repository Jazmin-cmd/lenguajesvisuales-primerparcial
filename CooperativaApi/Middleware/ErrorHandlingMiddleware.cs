using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace CooperativaApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Siempre definimos las mismas propiedades para evitar error de tipo
                var response = new
                {
                    message = _env.IsDevelopment() ? ex.Message : "Ocurrió un error en el servidor.",
                    stackTrace = _env.IsDevelopment() ? ex.StackTrace : string.Empty
                };

                var result = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(result);
            }
        }
    }
}
