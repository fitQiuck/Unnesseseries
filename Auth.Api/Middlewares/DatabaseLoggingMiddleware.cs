using Auth.Service.Interfaces;

namespace Auth.Api.Middlewares;

public class DatabaseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public DatabaseLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        this._next = next;  
        this._serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using var scope = _serviceProvider.CreateScope();
        var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

        try
        {
            //error
            await _next(context);

            await logService.LogAsync(
                action: "Request completed",
                tableName: null,
                performedBy: context.User?.Identity?.Name ?? "Anonymous",
                description: $"{context.Request.Method} {context.Request.Path}",
                method: context.Request.Method,
                ipAddress: context.Connection.RemoteIpAddress?.ToString()
            );
        }
        catch (Exception ex)
        {
            await logService.LogAsync(
                action: "Error occurred",
                tableName: null,
                performedBy: context.User?.Identity?.Name ?? "System",
                description: ex.Message,  // Optionally log the exception message
                method: context.Request.Method,
                ipAddress: context.Connection.RemoteIpAddress?.ToString()
            );
            //error
            throw;
        }
    }
}
