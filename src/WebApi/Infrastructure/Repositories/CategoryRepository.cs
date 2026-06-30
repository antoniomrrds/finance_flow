using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Categories;
using WebApi.Infrastructure.Data;

namespace WebApi.Infrastructure.Repositories;

public class CategoryReaderRepository(AppDbContext db) : ICategoryReaderRepository
{
    public Task<bool> HasCategoryWithNameAsync(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        return db.Categories.Where(c => c.Name == name).AnyAsync(cancellationToken);
    }
}
