namespace WebApi.Domain.Categories;

internal interface ICategoryWriterRepository
{
    Task<Guid> AddAsync(Category category, CancellationToken cancellationToken = default);
}

public interface ICategoryCheckRepository
{
    Task<bool> HasCategoryWithNameAsync(
        string name,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default
    );
}
