using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyApp1.Application.Common;
using MyApp1.Application.Exceptions;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyApp1.API.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status400BadRequest,
                BusinessRuleException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
            context.Response.StatusCode = statusCode;

            var response = ApiResponse<object>.FailResponse(
        statusCode,
        exception.Message ?? "Internal Server Error",
        null // you can optionally pass extra message or null
             );

            var result = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(result);
        }

    }
}
