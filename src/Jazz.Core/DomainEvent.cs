namespace Jazz.Core;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime Timestamp { get; }
}

/// Domain Events devem ser imutáveis!
public abstract record DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
    }
    
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime Timestamp { get; init; } = DateTimeProvider.Now;
}