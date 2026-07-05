using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace WebApi.Extensions.Docs;

public static class SwaggerSetup
{
    public static void AddOpenApiConfig(this IServiceCollection services)
    {
        foreach (ApiVersionsType version in ApiVersions.All)
        {
            services.AddOpenApi(
                version.Name,
                options =>
                {
                    options.CreateSchemaReferenceId = jsonTypeInfo =>
                    {
                        Type type = jsonTypeInfo.Type;

                        if (type.IsNested && type.DeclaringType is not null)
                        {
                            return type.DeclaringType.Name + type.Name;
                        }

                        return OpenApiOptions.CreateDefaultSchemaReferenceId(jsonTypeInfo);
                    };

                    options.AddDocumentTransformer(
                        (document, _, _) =>
                        {
                            document.Info = new OpenApiInfo
                            {
                                Title = $"{ApiInfo.Title}",
                                Version = version.Name,
                                Description = version.Description,
                                TermsOfService = new Uri($"{ApiInfo.TermsOfService}"),
                                Contact = new OpenApiContact
                                {
                                    Name = $"{ApiInfo.ContactName}",
                                    Email = $"{ApiInfo.ContactEmail}",
                                    Url = new Uri($"{ApiInfo.ContactUrl}"),
                                },
                                License = new OpenApiLicense
                                {
                                    Name = $"{ApiInfo.LicenseName}",
                                    Url = new Uri($"{ApiInfo.LicenseUrl}"),
                                },
                            };

                            return Task.CompletedTask;
                        }
                    );
                }
            );
        }
    }
}
