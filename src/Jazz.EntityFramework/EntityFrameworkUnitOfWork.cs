using DotNetCore.CAP;
using Jazz.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace Jazz.EntityFramework;

public sealed class EntityFrameworkUnitOfWork<TDbContext> : IDisposable, IUnitOfWork, IAggregateSavedHandler
    where TDbContext : DbContext
{
    private static readonly ILogger Log = Serilog.Log.ForContext<EntityFrameworkUnitOfWork<TDbContext>>();
    private static readonly object Sync = new object();
    private readonly TDbContext _dbContext;
    private readonly ICapPublisher _publisher;
    private readonly IDbContextTransaction? _transaction;
    private readonly Queue<IDomainEvent> _messages = new Queue<IDomainEvent>();
    private bool _disposed = false;

    public EntityFrameworkUnitOfWork(TDbContext dbContext, ICapPublisher publisher)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _transaction = _dbContext.Database.BeginTransaction(publisher, autoCommit: false);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null) throw new NullReferenceException("Database transaction does not exist.");
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await PublishMessages(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
            Log.Debug("Database transaction {TransactionId} committed", _transaction.TransactionId);
        }
        catch (Exception ex)
        {
            await _transaction.RollbackAsync(cancellationToken);
            Log.Error(ex, "Database transaction {TransactionId} rolled back", _transaction.TransactionId);
            throw;
        }
    }

    private async Task PublishMessages(CancellationToken cancellationToken)
    {
        while (_messages.TryDequeue(out var msg))
        {
            await _publisher.PublishAsync(msg.GetType().Name, msg, cancellationToken: cancellationToken);
        }
    }

    public Task HandleAggregateSaved(object aggregate)
    {
        if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));
        var events = GetEvents(aggregate);

        while (events.TryDequeue(out var e))
        {
            _messages.Enqueue(e);
        }

        return Task.CompletedTask;
    }

    private static Queue<IDomainEvent> GetEvents(object aggregate) => ((IHasEvents)aggregate).GetEvents();

    public void Dispose()
    {
        lock (Sync)
        {
            if (_transaction == null || _disposed) return;
            Log.Debug("Closing database transaction {TransactionId}", _transaction.TransactionId);
            _transaction.Dispose();
            _disposed = true;
        }
    }
}