using WebApi.Infrastructure.Persistence.Data;
using WebApi.Tests.Infrastructure.Factories;

namespace WebApi.Tests.Infrastructure.Helpers;

public abstract class RepositoryTestBase : IDisposable
{
    protected AppDbContext Db { get; }

    protected RepositoryTestBase()
    {
        Db = TestDbFactory.Create();
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    protected async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await Db.Set<TEntity>().AddAsync(entity);
        await Db.SaveChangesAsync();
    }

    protected async Task InsertBatchRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        Db.Set<TEntity>().AddRange(entities);
        await Db.SaveChangesAsync();
    }
}
