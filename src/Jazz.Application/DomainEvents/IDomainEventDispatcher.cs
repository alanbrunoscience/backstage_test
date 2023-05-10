using Jazz.Core;

namespace Jazz.Application.DomainEvents;

public interface IDomainEventDispatcher
{
    Task Dispatch(params IDomainEvent[]? events);
}