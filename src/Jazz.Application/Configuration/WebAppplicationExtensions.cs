using Jazz.Application.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Jazz.Application.Configuration;

public static class WebAppplicationExtensions
{
    public static WebApplication UseUserNameLogging(this WebApplication app)
    {
        app.UseMiddleware<LogUserNameMiddleware>();
        return app;
    }

    public static WebApplication UseValidation(this WebApplication app)
    {
        app.UseMiddleware<ValidationMiddleware>();
        return app;
    }
}