using FluentValidation;
using MediatR;

namespace Jazz.Application.Handlers;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
        return await next();
    }
}
