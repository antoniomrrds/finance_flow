namespace SharedKernel.Pagination;

public sealed record PagedResponse<T>
{
    public IReadOnlyList<T> Data { get; init; } = [];
    public PagedMetadata Meta { get; init; } = default!;

    public static PagedResponse<T> Create(IReadOnlyList<T> data, PagedMetadata meta) =>
        new() { Data = data, Meta = meta };
}
