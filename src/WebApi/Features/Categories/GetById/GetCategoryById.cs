using SharedKernel;
using WebApi.Application.Abstractions.Messaging;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.GetById;

public static class GetCategoryById
{
    public sealed record Query(Guid CategoryId) : IQuery<CategoryResponse>;

    public sealed record CategoryResponse() : CommonCategoryProperties
    {
        public Guid Id { get; init; }

        public DateTime CreatedAt { get; init; }
    }

    internal class Handler : IQueryHandler<Query, CategoryResponse>
    {
        private readonly ICategoryRepository _repo;

        public Handler(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<CategoryResponse>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            Category? category = await _repo.GetByIdAsync(query.CategoryId, cancellationToken);
            return category is null
                ? CategoryErrors.NotFound(id: query.CategoryId.ToString())
                : category.ToResponse();
        }
    }
}

public static class CategoryExtension
{
    public static GetCategoryById.CategoryResponse ToResponse(this Category category) =>
        new()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description ?? string.Empty,
            CreatedAt = category.CreatedAt,
        };
}
