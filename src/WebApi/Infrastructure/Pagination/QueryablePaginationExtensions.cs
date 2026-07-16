namespace WebApi.Infrastructure.Pagination;

public static class QueryablePaginationExtensions
{
    public static async Task<(List<T> Items, PagedMetadata Meta)> ToPagedAsync<T>(
        this IOrderedQueryable<T> query,
        PaginationQuery pagination,
        CancellationToken cancellationToken = default
    )
    {
        int totalCount = await query.CountAsync(cancellationToken);

        List<T> items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        var meta = PagedMetadata.Create(pagination.PageNumber, pagination.PageSize, totalCount);

        return (items, meta);
    }
}
