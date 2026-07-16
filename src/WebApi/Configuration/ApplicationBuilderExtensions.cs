using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApi.Configuration.Docs;
using WebApi.Configuration.Versioning;

namespace WebApi.Configuration;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.UseDevelopmentTools();

        app.UseExceptionHandler();
        app.UseHttpsRedirection();

        RouteGroupBuilder api = ApiVersioningSetup.MapGroupVersionSet(app);

        api.MapHealthCheck();
        app.MapEndpoints(api);

        return app;
    }

    private static void UseDevelopmentTools(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // app.ApplyMigrations();
            app.UseOpenApiUi();
        }
    }

    private static void MapHealthCheck(this RouteGroupBuilder api)
    {
        api.MapGet(
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
