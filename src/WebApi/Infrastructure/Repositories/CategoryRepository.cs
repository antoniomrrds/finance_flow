using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Categories;
using WebApi.Infrastructure.Data;

namespace WebApi.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext db) : ICategoryRepository
{
    public Task<bool> HasCategoryWithNameAsync(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        return db.Categories.Where(c => c.Name == name).AnyAsync(cancellationToken);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await db.Categories.AddAsync(category, cancellationToken);
    }
}
