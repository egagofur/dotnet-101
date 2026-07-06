using System.Net;
using System.Text.Json;

namespace WarehouseApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Terjadi kesalahan sistem: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            ArgumentException => HttpStatusCode.BadRequest,          // 400 Bad Request
            InvalidOperationException => HttpStatusCode.Conflict,    // 409 Conflict
            KeyNotFoundException => HttpStatusCode.NotFound,         // 404 Not Found
            _ => HttpStatusCode.InternalServerError                  // 500 Internal Server Error
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            success = false,
            message = statusCode == HttpStatusCode.InternalServerError
                ? "Terjadi kesalahan internal pada server."
                : exception.Message
        };

        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}
