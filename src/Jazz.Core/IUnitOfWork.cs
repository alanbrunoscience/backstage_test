namespace Jazz.Core;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}