using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Cart_WebAPI.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            var response = httpContext.Response;

            var errorResponse = new ErrorResponse
            {
                Success = false,
                ErrorCode = (int)HttpStatusCode.InternalServerError,
                ErrorMessage = "An error occurred while processing your request. Please try again or contact support."
            };

            switch (exception)
            {
                case ApplicationException appEx:
                    if (appEx.Message.Contains("Invalid token"))
                    {
                        errorResponse.ErrorCode = (int)HttpStatusCode.Forbidden;
                        errorResponse.ErrorMessage = appEx.Message;
                        break;
                    }
                    errorResponse.ErrorCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.ErrorMessage = appEx.Message;
                    break;
                case KeyNotFoundException keyEx:
                    errorResponse.ErrorCode = (int)HttpStatusCode.NotFound;
                    errorResponse.ErrorMessage = keyEx.Message;
                    break;
                case ArgumentNullException ex:
                    errorResponse.ErrorCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.ErrorMessage = ex.Message;
                    break;
                case ArgumentException ex:
                    errorResponse.ErrorCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.ErrorMessage = ex.Message;
                    break;
                case InvalidOperationException ex:
                    errorResponse.ErrorCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.ErrorMessage = ex.Message;
                    break;

                case UnauthorizedAccessException authEx:
                    errorResponse.ErrorCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.ErrorMessage = authEx.Message;
                    break;

                default:
                    errorResponse.ErrorCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.ErrorMessage = "Internal Server Error. Please check logs!";
                    break;
            }
            _logger.LogError(exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await httpContext.Response.WriteAsync(result);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GlobalExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}
