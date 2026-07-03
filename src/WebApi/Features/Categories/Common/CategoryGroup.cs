using System.Globalization;
using WebApi.Endpoints;

namespace WebApi.Features.Categories.Common;

public class CategoryGroup : IEndpointGroup
{
    public const string BaseRoute = "categories";
    public static readonly string Tag = BaseRoute.ToUpper(CultureInfo.InvariantCulture);

    public RouteGroupBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapGroup($"/{BaseRoute}").WithTags(Tag);
    }
}
