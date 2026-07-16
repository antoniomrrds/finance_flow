using WebApi.Domain.Categories;

namespace WebApi.Features.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
