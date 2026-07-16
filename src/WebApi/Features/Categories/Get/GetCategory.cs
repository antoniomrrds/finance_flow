using WebApi.Configuration;
using WebApi.Configuration.Docs;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Shared;
using WebApi.Infrastructure.Http;

namespace WebApi.Features.Categories.Get;

public static class GetCategories
{
    public sealed record Query(int PageNumber, int PageSize, string? Name)
        : IQuery<PagedResponse<Response>>;

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
                    "/",
                    async (
                        string? name,
                        IQueryHandler<Query, PagedResponse<Response>> handler,
                        CancellationToken cancellationToken,
                        int pageNumber = 1,
                        int pageSize = 25
                    ) =>
                    {
                        Query query = new(pageNumber, pageSize, name);
                        Result<PagedResponse<Response>> result = await handler.Handle(
                            query,
                            cancellationToken
                        );

                        return result.Match(Results.Ok, CustomProblemResults.Problem);
                    }
                )
                .WithSummary("Listar categorias")
                .ProducesOk<PagedResponse<Response>>("Categorias encontradas com sucesso.")
                .ProducesInternalServerError();
        }
    }

    internal sealed class Handler(ICategoryRepository repo)
        : IQueryHandler<Query, PagedResponse<Response>>
    {
        public async Task<Result<PagedResponse<Response>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            PaginationQuery pagination = new()
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
            };

            (List<Category> items, PagedMetadata meta) = await repo.GetPagedAsync(
                pagination,
                query.Name,
                cancellationToken
            );

            var data = items.Select(c => c.ToResponse<Response>()).ToList();

            return PagedResponse<Response>.Create(data, meta);
        }
    }
}
