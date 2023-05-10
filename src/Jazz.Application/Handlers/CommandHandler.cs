using MediatR;

namespace Jazz.Application.Handlers;

public interface ICommandHandler<in TCommand, TCommandResult> : IRequestHandler<TCommand, TCommandResult> where TCommand : ICommand<TCommandResult>
{
}

public interface ICommand<out TCommandResult> : IRequest<TCommandResult>
{
}