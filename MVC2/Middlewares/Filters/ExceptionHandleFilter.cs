// في مجلد MVC2.Middlewares.Filters/ExceptionHandleFilter.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace MVC2.Middlewares.Filters
{
    public class ExceptionHandleFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionHandleFilter> _logger;

        public ExceptionHandleFilter(ILogger<ExceptionHandleFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var currentTime = DateTime.Now; // 07:12 PM EEST, Sunday, October 05, 2025

            // سجل الاستثناء مع تفاصيل زيادة
            _logger.LogError(exception, "An error occurred at {Time}: {Message}", currentTime, exception.Message);

            var errorDetails = new
            {
                Message = "An unexpected error occurred!",
                ExceptionMessage = exception.Message,
                Source = exception.Source,
                Time = currentTime.ToString("hh:mm tt zzz, dddd, MMMM dd, yyyy") // e.g., 07:12 PM EEST, Sunday, October 05, 2025
            };

            // تحقق إذا كان الـ Request يطلب Json (Ajax)
            var isAjaxRequest = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                               context.HttpContext.Request.Headers["Accept"].Any(h => h.Contains("application/json"));

            if (isAjaxRequest)
            {
                context.Result = new JsonResult(errorDetails)
                {
                    StatusCode = 500 // Internal Server Error
                };
            }
            else
            {
                context.Result = new ViewResult
                {
                    ViewName = "Error",
                    ViewData = new Microsoft.AspNetCore.Mvc.ViewDataDictionary(
                        new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                        context.ModelState)
                    {
                        { "ErrorMessage", errorDetails.Message },
                        { "ExceptionDetails", errorDetails.ExceptionMessage },
                        { "ErrorTime", errorDetails.Time }
                    }
                };
            }

            context.ExceptionHandled = true;
        }
    }
}