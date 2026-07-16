using WebApi.Configuration;
using WebApi.Configuration.Docs;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Shared;
using WebApi.Infrastructure.Http;

namespace WebApi.Features.Categories.GetById;

public static class GetCategoryById
{
    public sealed record Query(Guid Id) : IQuery<Response>;

    public sealed record Response : ICategoryResponse
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string? Description { get; init; }
        public DateTime CreatedAt { get; init; }
    }

    public sealed class Endpoint : IEndpoint<CategoryGroup>
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(
                    "/{id:guid}",
                    async (
                        Guid id,
                        IQueryHandler<Query, Response> handler,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        Query query = new(id);
                        Result<Response> result = await handler.Handle(query, cancellationToken);

                        return result.Match(Results.Ok, CustomProblemResults.Problem);
                    }
                )
                .WithSummary("Buscar uma categoria")
                .ProducesOk<Response>("Categoria encontrada com sucesso.")
                .ProducesNotFound("Categoria não encotrada.")
                .ProducesInternalServerError();
        }
    }

    internal sealed class Handler(ICategoryRepository repo) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            Category? category = await repo.GetByIdAsync(query.Id, cancellationToken);
            return category is null
                ? CategoryErrors.NotFound(id: query.Id.ToString())
                : category.ToResponse<Response>();
        }
    }
}
