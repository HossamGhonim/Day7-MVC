using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MVC2.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // كمل البايبلاين
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Roogram Execption");

                // إعادة توجيه لصفحة Error
                context.Response.Redirect($"/Home/Error?message={Uri.EscapeDataString(ex.Message)}");
            }
        }
    }

    // Extension عشان نضيفه بسهولة
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
