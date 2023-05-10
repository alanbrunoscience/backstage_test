using LinqBuilder;

namespace Jazz.Core;

public abstract class RepositoryBase<TId, TAggregateRoot> : IRepository<TId, TAggregateRoot>
    where TId : IEquatable<TId>
    where TAggregateRoot : class, IAggregateRoot<TId>
{
    public event Func<object, AggregateRootSavedEventArgs, Task>? AggregateRootSaved;

    public async Task SaveAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default)
    {
        await InternalSaveAsync(aggregate, cancellationToken);
        OnAggregateRootSaved(new AggregateRootSavedEventArgs(aggregate));
    }

    public async Task<TAggregateRoot?> LoadAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await InternalLoadAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<TAggregateRoot>> FindAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default)
    {
        return await InternalFindAsync(specification, cancellationToken);
    }

    protected abstract Task InternalSaveAsync(TAggregateRoot aggregate, CancellationToken cancellationToken);

    protected abstract Task<TAggregateRoot?> InternalLoadAsync(TId id, CancellationToken cancellationToken);

    protected abstract Task<IEnumerable<TAggregateRoot>> InternalFindAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken);

    private void OnAggregateRootSaved(AggregateRootSavedEventArgs e)
    {
        if (e == null) throw new ArgumentNullException(nameof(e));
        AggregateRootSaved?.Invoke(this, e);
    }
}