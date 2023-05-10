using Jazz.Core;
using LinqBuilder;
using Microsoft.EntityFrameworkCore;

namespace Jazz.EntityFramework;

public class EntityFrameworkRepository<TId, TAggregateRoot, TDbContext> : RepositoryBase<TId, TAggregateRoot>
    where TId : IEquatable<TId>
    where TAggregateRoot : class, IAggregateRoot<TId>
    where TDbContext : DbContext
{
    private DbSet<TAggregateRoot> Collection { get; }

    protected EntityFrameworkRepository(TDbContext dbContext)
    {
        if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
        Collection = dbContext.Set<TAggregateRoot>();
    }

    private bool Exists(TAggregateRoot aggregate) => Collection.Any(x => x.Id.Equals(aggregate.Id));

    protected override async Task InternalSaveAsync(TAggregateRoot aggregate, CancellationToken cancellationToken)
    {
        if (Exists(aggregate))
            await Task.FromResult(Collection.Update(aggregate));
        else
            await Collection.AddAsync(aggregate, cancellationToken);
    }

    protected override async Task<TAggregateRoot?> InternalLoadAsync(TId id, CancellationToken cancellationToken) =>
        await Collection.FindAsync(id, cancellationToken);

    protected override async Task<IEnumerable<TAggregateRoot>> InternalFindAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken) =>
        await Task.FromResult(Collection.Where(specification).AsEnumerable());
}