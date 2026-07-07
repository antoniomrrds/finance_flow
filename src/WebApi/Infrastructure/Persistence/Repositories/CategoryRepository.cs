using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Categories;
using WebApi.Infrastructure.Persistence.Data;

namespace WebApi.Infrastructure.Persistence.Repositories;

public class CategoryRepository(AppDbContext db) : ICategoryRepository
{
    public Task<bool> HasCategoryWithNameAsync(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        return db.Categories.Where(c => c.Name == name).AnyAsync(cancellationToken);
    }

    public Task<Category?> GetByIdAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await db.Categories.AddAsync(category, cancellationToken);
    }
}
