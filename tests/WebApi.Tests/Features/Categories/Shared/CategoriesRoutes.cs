namespace WebApi.Tests.Features.Categories.Shared;

internal static class CategoriesRoutes
{
    public const string BasePath = "/api/v1/categories";

    public static Uri GetById(Guid id) => new($"{BasePath}/{id}", UriKind.Relative);

    public static Uri Update(Guid id) => new($"{BasePath}/{id}", UriKind.Relative);

    public static Uri Delete(Guid id) => new($"{BasePath}/{id}", UriKind.Relative);

    public static Uri GetAll => new(BasePath, UriKind.Relative);
}
