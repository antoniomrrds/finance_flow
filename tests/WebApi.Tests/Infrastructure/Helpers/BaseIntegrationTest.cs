using Microsoft.Extensions.DependencyInjection;
using WebApi.Infrastructure.Persistence.Data;
using WebApi.Tests.Infrastructure.Factories;

namespace WebApi.Tests.Infrastructure.Helpers;

public abstract class BaseIntegrationTest : IClassFixture<SqliteTestWebAppFactory>, IAsyncLifetime
{
    protected HttpClient Client { get; }
    protected AppDbContext Db { get; }

    protected BaseIntegrationTest(SqliteTestWebAppFactory factory)
    {
        Client = factory.CreateClient();

        IServiceScope scope = factory.Services.CreateScope();

        Db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    private async Task ResetDatabaseAsync()
    {
        await Db.Database.EnsureDeletedAsync();
        await Db.Database.EnsureCreatedAsync();
    }

    public async Task InitializeAsync()
    {
        await ResetDatabaseAsync();
        Db.ChangeTracker.Clear();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    protected async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await Db.Set<TEntity>().AddAsync(entity);
        await Db.SaveChangesAsync();
    }
}
