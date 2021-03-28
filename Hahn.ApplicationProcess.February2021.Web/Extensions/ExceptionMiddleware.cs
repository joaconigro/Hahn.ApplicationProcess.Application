using Hahn.Web.Dtos;
using Hahn.Web.Log;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hahn.Web.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogManager _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogManager logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorVM = new ErrorDetailsDto(context.Response.StatusCode, exception);
            var options = new JsonSerializerOptions { WriteIndented = true };
            var errorJson = JsonSerializer.Serialize(errorVM, options);
            return context.Response.WriteAsync(errorJson);
        }
    }
}
