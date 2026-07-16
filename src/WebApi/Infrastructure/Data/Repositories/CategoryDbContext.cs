using WebApi.Domain.Categories;
using WebApi.Infrastructure.Pagination;

namespace WebApi.Infrastructure.Data.Repositories;

public class CategoryDbContext(AppDbContext db)
{
    public async Task<(List<Category> Items, PagedMetadata Meta)> GetPagedAsync(
        PaginationQuery pagination,
        string? nameFilter,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Category> query = db.Categories.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(nameFilter))
        {
            query = query.Where(c => c.Name.Contains(nameFilter));
        }

        return await query.OrderBy(c => c.Name).ToPagedAsync(pagination, cancellationToken);
    }
}
