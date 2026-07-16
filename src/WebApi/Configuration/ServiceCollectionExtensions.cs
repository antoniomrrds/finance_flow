using FluentValidation;
using WebApi.Configuration.Docs;
using WebApi.Configuration.Versioning;
using WebApi.Domain.Abstractions;
using WebApi.Features.Abstractions.Behaviors;
using WebApi.Features.Abstractions.Data;
using WebApi.Infrastructure.Data;
using WebApi.Infrastructure.Http;

namespace WebApi.Configuration;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string connection = GetConnectionString(configuration);

        services.AddInfrastructure(connection).AddFeatures().AddApi();
    }

    private static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connection
    )
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connection).UseSnakeCaseNamingConvention()
        );
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddHealthChecks().AddNpgSql(connection, "database");

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddEndpointsApiExplorer();
        services.AddVersioning();
        services.AddOpenApiConfig();

        return services;
    }

    private static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssembliesOf(typeof(ServiceCollectionExtensions))
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.Decorate(
            typeof(ICommandHandler<,>),
            typeof(ValidationDecorator.CommandHandler<,>)
        );
        services.Decorate(
            typeof(ICommandHandler<>),
            typeof(ValidationDecorator.CommandBaseHandler<>)
        );
        services.AddValidatorsFromAssembly(
            typeof(ServiceCollectionExtensions).Assembly,
            includeInternalTypes: true
        );

        return services;
    }

    private static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddEndpoints(Assembly.GetExecutingAssembly());

        return services;
    }

    private static string GetConnectionString(IConfiguration configuration) =>
        configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}
