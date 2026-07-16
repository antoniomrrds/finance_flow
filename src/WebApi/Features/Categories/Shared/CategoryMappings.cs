using System.Linq.Expressions;
using WebApi.Domain.Categories;

namespace WebApi.Features.Categories.Shared;

internal static class CategoryMappings
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

    public static Expression<Func<Category, TResponse>> ToResponseExpression<TResponse>()
        where TResponse : ICategoryResponse, new() =>
        category => new TResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
        };
}
