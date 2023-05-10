using LinqBuilder;

namespace Jazz.Core;

public interface IRepository<in TId, TAggregateRoot>
    where TId : IEquatable<TId>
    where TAggregateRoot : class, IAggregateRoot<TId>
{
    Task SaveAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default);
    
    Task<TAggregateRoot?> LoadAsync(TId id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<TAggregateRoot>> FindAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default);
    
    event Func<object, AggregateRootSavedEventArgs, Task>? AggregateRootSaved;
}

public class AggregateRootSavedEventArgs : EventArgs
{
    public AggregateRootSavedEventArgs(object aggregateRoot)
    {
        AggregateRoot = aggregateRoot ?? throw new ArgumentNullException(nameof(aggregateRoot));
    }

    public object AggregateRoot { get; }
}