using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApi.Endpoints;
using WebApi.Extensions.Docs;

namespace WebApi.Extensions;

public static class ApplicationSetup
{
    /// <summary>
    /// Cria o versionamento da API, mapeia o grupo /api/v{version} e
    /// registra todos os endpoints descobertos via reflection.
    /// </summary>
    public static void MapApiEndpoints(this WebApplication app)
    {
        RouteGroupBuilder api = ApiVersioningSetup.MapGroupVersionSet(app);

        api.MapHealthChecks();

        app.MapEndpoints(api);
    }

    private static void MapHealthChecks(this RouteGroupBuilder app)
    {
        app.MapGet(
                "/health",
                async (HttpContext context, HealthCheckService service) =>
                {
                    HealthReport report = await service.CheckHealthAsync();

                    context.Response.ContentType = "application/json";

                    await UIResponseWriter.WriteHealthCheckUIResponse(context, report);
                }
            )
            .MapToApiVersion(1)
            .WithTags("Health");
    }
}
