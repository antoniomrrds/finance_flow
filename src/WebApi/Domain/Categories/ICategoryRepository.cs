namespace WebApi.Domain.Categories;

public interface ICategoryRepository
{
    Task<bool> HasCategoryWithNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task AddAsync(Category category, CancellationToken cancellationToken = default);

    Task<CategoryUpdateOutcome> UpdateIfValidAsync(
        Category category,
        CancellationToken cancellationToken = default
    );
    Task<(List<Category> Items, PagedMetadata Meta)> GetPagedAsync(
        PaginationQuery pagination,
        string? nameFilter,
        CancellationToken cancellationToken = default
    );

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public enum CategoryUpdateOutcome
{
    Updated,
    NotFound,
    NameConflict,
}
