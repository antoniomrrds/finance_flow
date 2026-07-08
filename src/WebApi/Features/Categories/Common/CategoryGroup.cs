using System.Globalization;
using WebApi.Endpoints;

namespace WebApi.Features.Categories.Common;

public class CategoryGroup : IEndpointGroup
{
    private const string Tag = "Categories";
    private static readonly string BaseRoute = Tag.ToLower(CultureInfo.InvariantCulture);

    public RouteGroupBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapGroup($"/{BaseRoute}").WithTags(Tag).MapToApiVersion(1);
    }
}
