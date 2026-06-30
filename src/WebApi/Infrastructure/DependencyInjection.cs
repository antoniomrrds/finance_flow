using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data;

namespace WebApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found."
            );
        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionString));

        // RegisterRepositories(services);
        return services;
    }

    // private static void RegisterRepositories(IServiceCollection services)
    // {
    //     services.AddScoped<ICategoryReaderRepository,CategoryCheckRepository>();
    // }
}
