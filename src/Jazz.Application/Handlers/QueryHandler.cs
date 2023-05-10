using MediatR;

namespace Jazz.Application.Handlers;

public interface IQuery<out TQueryResult> : IRequest<TQueryResult>
{
}

public interface IQueryHandler<in TQuery, TQueryResult> : IRequestHandler<TQuery, TQueryResult> where TQuery : IQuery<TQueryResult>
{
}

public interface IPagedQuery
{
    int Skip { get; }
    int Size { get; }
}

public abstract record PagedQuery(int Skip = 0, int Size = 10) : IPagedQuery;

public interface IPagedResult<T>
{
    int ItemsCount { get; }
    int PageCount { get; }
    int CurrentPage { get; }
    IEnumerable<T> Items { get; }
    int Skip { get; }
    int Size { get; }
    int TotalCount { get; }
}

public record PagedResult<T>(IEnumerable<T> Items, int Skip, int Size, int TotalCount) : IPagedResult<T>
{
    public int ItemsCount => Items.Count();
    public int PageCount => (int)Math.Ceiling((decimal)TotalCount / Size);
    public int CurrentPage => (int)Math.Ceiling((decimal)Skip / Size) + 1;
}