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

    public async Task<Category?> GetByIdAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default
    )
    {
        return await db
            .Categories.AsNoTracking()
            .SingleOrDefaultAsync(c => c.Id == categoryId, cancellationToken: cancellationToken);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await db.Categories.AddAsync(category, cancellationToken);
    }

    public async Task<CategoryUpdateOutcome> UpdateIfValidAsync(
        Category category,
        CancellationToken cancellationToken = default
    )
    {
        int affectedRows = await db
            .Categories.Where(c => c.Id == category.Id)
            .Where(c =>
                !db.Categories.Any(other => other.Id != category.Id && other.Name == category.Name)
            )
            .ExecuteUpdateAsync(
                setters =>
                    setters
                        .SetProperty(c => c.Name, category.Name)
                        .SetProperty(c => c.Description, category.Description),
                cancellationToken
            );

        if (affectedRows > 0)
        {
            return CategoryUpdateOutcome.Updated;
        }

        bool exists = await db.Categories.AnyAsync(c => c.Id == category.Id, cancellationToken);

        return exists ? CategoryUpdateOutcome.NameConflict : CategoryUpdateOutcome.NotFound;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        int affectedRows = await db
            .Categories.Where(c => c.Id == id)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);

        return affectedRows > 0;
    }
}
