using Jazz.Application.DomainEvents;
using Jazz.Core;

namespace Jazz.Covenant.Application.Configuration;

public static class EventHandlingExtensions
{
    public static IRepository<TId, TAggregate> AddEventHandling<TId, TAggregate>(this IRepository<TId, TAggregate> repository, IDomainEventDispatcher dispatcher)
        where TId : IEquatable<TId>
        where TAggregate : class, IAggregateRoot<TId>
    {
        if (repository == null) throw new ArgumentNullException(nameof(repository));
        if (dispatcher == null) throw new ArgumentNullException(nameof(dispatcher));
        repository.AggregateRootSaved +=
            (_, args) =>
            {
                var events = ((IHasEvents)args.AggregateRoot).GetEvents().ToArray();
                dispatcher.Dispatch(events);
                Serilog.Log.Debug("Domain events {@DomainEvents}", events);
                return Task.CompletedTask;
            };
        return repository;
    }
}