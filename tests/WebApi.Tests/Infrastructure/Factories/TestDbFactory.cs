using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data;

namespace WebApi.Tests.Infrastructure.Factories;

public static class TestDbFactory
{
    public static AppDbContext Create()
    {
        SqliteConnection connection = new("DataSource=:memory:");

        connection.Open();

        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        AppDbContext context = new(options);

        context.Database.EnsureCreated();

        context.SaveChanges();

        return context;
    }
}
