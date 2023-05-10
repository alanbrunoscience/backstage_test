using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Jazz.Application.Middlewares;

public sealed class ValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next) =>
        _next = next ?? throw new ArgumentNullException(nameof(next));

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            var result = Results.ValidationProblem(detail: ex.Message, 
                                                   errors: ex.Errors.ToDictionary(k => k.PropertyName, v => new[] { v.ErrorMessage }));
            await result.ExecuteAsync(context);
        }
    }
}