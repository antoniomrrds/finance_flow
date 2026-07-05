using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebApi.Extensions.Docs;

public static class SwaggerUiSetup
{
    public static void UseOpenApiUi(this WebApplication app)
    {
        app.MapOpenApi();

        app.UseSwaggerUI(options =>
        {
            foreach (ApiVersionsType version in ApiVersions.All)
            {
                options.SwaggerEndpoint(
                    $"/openapi/{version.Name}.json",
                    $"FinanceFlow {version.Description} - {version.Name} "
                );

                options.DocExpansion(DocExpansion.List);
            }
        });
    }
}
