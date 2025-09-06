using Auth.Service.Interfaces;

namespace Auth.Api.Middlewares;

public class TokenValidationMidlleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMidlleware(RequestDelegate _next)
    {
        this._next = _next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
    {
        var path = context.Request.Path.Value?.ToLower();

        // Agar bu /api/auth/login bo‘lsa, token tekshirmay o‘tkazamiz
        if (path == "/api/auth/login")
        {
            await _next(context);
            return;
        }

        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();

            var tokenExists = await tokenService.CheckTokenExistsAsync(token);

            if (!tokenExists)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid token");
                return;
            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Bearer token required");
            return;
        }
        //error
        await _next(context);
    }
}
