using Auth.Service.Exceptions;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace Auth.Api.Middlewares;

public class ExceptionHandlerMiddleWare
{
    private readonly ILogger<ExceptionHandlerMiddleWare> logger;
    private readonly RequestDelegate next;

    public ExceptionHandlerMiddleWare(ILogger<ExceptionHandlerMiddleWare> _logger,
        RequestDelegate requestDelegate)
    {
        this.logger = _logger;
        this.next = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        try
        {
            await next.Invoke(context);
        }
        catch (HttpStatusCodeException ex)
        {
            await HandleException(context, ex.Code, ex.Message);
        }
        catch (Exception ex)
        {
            //Log
            logger.LogError(ex.ToString());

            await HandleException(context, 500, ex.Message);
        }
    }

    public async Task HandleException(HttpContext context, int code, string message)
    {
        context.Response.StatusCode = code;
        await context.Response.WriteAsJsonAsync(new
        {
            Code = code,
            Message = message
        });
    }
}
