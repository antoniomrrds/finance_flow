namespace WebApi.Features.Abstractions.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}

public interface IEndpoint<TGroup> : IEndpoint
    where TGroup : IEndpointGroup, new();

public interface IEndpointGroup
{
    RouteGroupBuilder Map(IEndpointRouteBuilder app);
}
