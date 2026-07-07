namespace WebApi.Domain.Categories;

public interface ICategoryRepository
{
    Task<bool> HasCategoryWithNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
}
