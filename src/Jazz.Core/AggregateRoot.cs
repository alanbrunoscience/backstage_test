namespace Jazz.Core;

public interface IAggregateRoot<out TId> : IEntity<TId>, IHasEvents where TId : IEquatable<TId>
{
    DateTime CreatedAt { get; }
    DateTime ModifiedAt { get; }
}

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId> where TId : IEquatable<TId>
{
    protected AggregateRoot()
    {
    }

    private Queue<IDomainEvent> EventsQueue { get; } = new Queue<IDomainEvent>();

    protected AggregateRoot(TId id) : base(id)
    {
        UpdateModifiedDate();
        CreatedAt = ModifiedAt;
    }

    public DateTime CreatedAt { get; private set; } = DateTimeProvider.Now;
    public DateTime ModifiedAt { get; private set; } = DateTimeProvider.Now;

    protected void UpdateModifiedDate() => ModifiedAt = DateTimeProvider.Now;

    protected void Raise(IDomainEvent @event)
    {
        if (@event == null) throw new ArgumentNullException(nameof(@event));
        EventsQueue.Enqueue(@event);
    }

    public Queue<IDomainEvent> GetEvents() => EventsQueue;
}