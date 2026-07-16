using WebApi.Domain.Categories;
using WebApi.Features.Categories.Create;
using WebApi.Features.Categories.Update;

namespace WebApi.Tests.Domain.Categories;

internal static class CategoryExtensions
{
    extension(Category category)
    {
        internal CreateCategory.Command ToCreateCommand() =>
            new() { Name = category.Name, Description = category.Description ?? string.Empty };

        internal UpdateCategory.Command ToUpdateCommand() =>
            new()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description ?? string.Empty,
            };
    }
}
