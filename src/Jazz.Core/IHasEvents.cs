namespace Jazz.Core;

public interface IHasEvents
{
    Queue<IDomainEvent> GetEvents();
}