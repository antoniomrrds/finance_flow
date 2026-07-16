using WebApi.Domain.Categories;

namespace WebApi.Features.Categories.Shared;

internal static class CategoryExtensions
{
    public static TResponse ToResponse<TResponse>(this Category category)
        where TResponse : ICategoryResponse, new() =>
        new()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
        };
}
