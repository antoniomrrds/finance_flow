using WebApi.Domain.Abstractions;
using WebApi.Domain.Categories;
using WebApi.Features.Abstractions.Data;

namespace WebApi.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options),
        IApplicationDbContext,
        IUnitOfWork
{
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
    }
}
