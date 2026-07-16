using Asp.Versioning;
using Asp.Versioning.Builder;

namespace WebApi.Configuration.Versioning;

public record ApiVersionsType(ApiVersion Version, string Name, string Description);

public static class ApiVersions
{
    public static readonly ApiVersion V1 = new(1);
    public static readonly ApiVersion V2 = new(2);

    public static IReadOnlyList<ApiVersionsType> All =>
        [
            new(V1, "v1", "Versão inicial da API"),
            // ,new(V2, "v2", "Versão atualizada da API")
        ];
}

public static class ApiVersioningSetup
{
    public static void AddVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = ApiVersions.V1;
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version")
                );
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
    }

    public static RouteGroupBuilder MapGroupVersionSet(WebApplication app)
    {
        ApiVersionSet versionSet = app.NewApiVersionSet()
            .HasApiVersion(ApiVersions.V1)
            // .HasApiVersion(ApiVersions.V2)
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder api = app.MapGroup("/api/v{version:apiVersion}")
            .WithApiVersionSet(versionSet);
        return api;
    }
}
