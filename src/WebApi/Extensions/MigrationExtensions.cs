using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Persistence.Data;

namespace WebApi.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        bool apply = configuration.GetValue<bool>("Database:ApplyMigrations");

        if (!apply)
        {
            return;
        }

        using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
}
