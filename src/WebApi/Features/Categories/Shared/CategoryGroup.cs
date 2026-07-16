using System.Globalization;

namespace WebApi.Features.Categories.Shared;

public class CategoryGroup : IEndpointGroup
{
    private const string Tag = "Categories";
    private static readonly string BaseRoute = Tag.ToLower(CultureInfo.InvariantCulture);

    public RouteGroupBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapGroup($"/{BaseRoute}").WithTags(Tag).MapToApiVersion(1);
    }
}
