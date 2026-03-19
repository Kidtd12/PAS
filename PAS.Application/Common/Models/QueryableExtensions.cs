namespace Application.Common.Models;

public static class QueryableExtensions
{
    public static Task<PaginatedList<T>> PaginatedListAsync<T>(this IQueryable<T> queryable, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return PaginatedList<T>.CreateAsync(queryable, pageNumber, pageSize, cancellationToken);
    }
}
