using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MVC2.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("➡ Request: {method} {url}",
                context.Request?.Method,
                context.Request?.Path.Value);

            var start = DateTime.UtcNow;

            await _next(context); // كمل البايبلاين

            var duration = DateTime.UtcNow - start;

            // 📌 Log Response
            _logger.LogInformation("⬅ Response: {statusCode} (took {time} ms)",
                context.Response?.StatusCode,
                duration.TotalMilliseconds);
        }
    }

    // Extension عشان نضيفه بسهولة
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
