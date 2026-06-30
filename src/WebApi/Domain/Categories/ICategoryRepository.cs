namespace WebApi.Domain.Categories;

public interface ICategoryWriterRepository
{
    Task<Guid> AddAsync(Category category, CancellationToken cancellationToken = default);
}

public interface ICategoryReaderRepository
{
    Task<bool> HasCategoryWithNameAsync(string name, CancellationToken cancellationToken = default);
}
