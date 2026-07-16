using WebApi.Infrastructure.Data;
using WebApi.Tests.Infrastructure.Factories;

namespace WebApi.Tests.Infrastructure.TestBase;

public abstract class DbContextTestBase : IDisposable
{
    protected AppDbContext Db { get; }

    protected DbContextTestBase()
    {
        Db = TestDbFactory.Create();
    }

    public void Dispose() => Db.Dispose();

    protected async Task SeedAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await Db.Set<TEntity>().AddAsync(entity);
        await Db.SaveChangesAsync();
    }

    protected async Task SeedRangeAsync<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        Db.Set<TEntity>().AddRange(entities);
        await Db.SaveChangesAsync();
    }
}
