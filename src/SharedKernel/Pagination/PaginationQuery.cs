namespace SharedKernel.Pagination;

public sealed record PaginationQuery
{
    private const int DefaultPageSize = 25;
    private const int MaxPageSize = 100;

    private readonly int _pageSize = DefaultPageSize;
    private readonly int _pageNumber = 1;

    public int PageNumber
    {
        get => _pageNumber;
        init => _pageNumber = value <= 0 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        init =>
            _pageSize = value switch
            {
                <= 0 => DefaultPageSize,
                > MaxPageSize => MaxPageSize,
                _ => value,
            };
    }
}
