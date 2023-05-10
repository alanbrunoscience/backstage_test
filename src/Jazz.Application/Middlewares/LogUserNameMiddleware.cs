using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Jazz.Application.Middlewares;

public sealed class LogUserNameMiddleware
{
    private readonly RequestDelegate _next;

    public LogUserNameMiddleware(RequestDelegate next) => 
        _next = next ?? throw new ArgumentNullException(nameof(next));

    public Task Invoke(HttpContext context)
    {
        var username = context.User.Identity?.Name;
        if (!string.IsNullOrWhiteSpace(username)) LogContext.PushProperty("Username", username);
        return _next(context);
    }
}