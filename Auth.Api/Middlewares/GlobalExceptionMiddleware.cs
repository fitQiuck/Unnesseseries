using Auth.Domain.Entities.ErrorsDetails;
using Auth.Service.Exceptions;
using System.Text.Json;

namespace Auth.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IServiceProvider _serviceProvider;

    public GlobalExceptionMiddleware(RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IServiceProvider serviceProvider)
    {
        _next = next;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            //error
            await _next.Invoke(context);
        }
        catch (HttpStatusCodeException ex)
        {
            await HandleExceptionAsync(context, ex, ex.Code);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex, 500);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
    {
        // Set response
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            Code = statusCode,
            Message = GetUserFriendlyMessage(exception, statusCode),
            Timestamp = DateTime.UtcNow
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }

    private ErrorDetails GetErrorDetails(HttpContext context, Exception exception, int statusCode)
    {
        return new ErrorDetails
        {
            Timestamp = DateTime.UtcNow,
            StatusCode = statusCode,
            Path = context.Request.Path,
            Method = context.Request.Method,
            UserAgent = context.Request.Headers.UserAgent.ToString(),
            IpAddress = context.Connection.RemoteIpAddress?.ToString(),
            ExceptionType = exception.GetType().Name,
            Message = exception.Message,
            StackTrace = exception.StackTrace,
            InnerException = exception.InnerException?.Message
        };
    }

    private string GetUserFriendlyMessage(Exception exception, int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad request. Please check your input.",
            401 => "Unauthorized. Please login.",
            403 => "Forbidden. You don't have permission.",
            404 => "Resource not found.",
            500 => "Internal server error. Please try again later.",
            _ => "An error occurred. Please contact support."
        };
    }
}
