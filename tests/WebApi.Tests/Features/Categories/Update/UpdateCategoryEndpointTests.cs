using WebApi.Domain.Categories;
using WebApi.Features.Categories.update;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Features.Categories.Common;
using WebApi.Tests.Infrastructure.Factories;
using WebApi.Tests.Infrastructure.Helpers;
using WebApi.Tests.Infrastructure.Http;

namespace WebApi.Tests.Features.Categories.Update;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(UpdateCategory))]
public class UpdateCategoryEndpointTests(SqliteTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private readonly Category _category = CategoryFixture.GetCategory(true);

    [Fact]
    public async Task Update_WhenGuidIsEmpty_ShouldReturn400WithError()
    {
        // Arrange
        UpdateCategory.Command command = _category.ToUpdateCommand();
        // Act
        HttpResponseMessage httpResponse = await Client.PutAsJsonAsync(
            CategoriesRoutes.Update(Guid.Empty),
            command
        );
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        // Assert
        ProblemDetailsResponse problem =
            await httpResponse.GetErrorResponse<ProblemDetailsResponse>();
        problem.Errors.ShouldNotBeNull();
        string[] error = problem.Errors.First().Value;
        error.First().ShouldBe("Informe um Id vàlido.");
    }

    [Fact]
    public async Task Update_WhenTheNameExists_ShouldReturn409WithError()
    {
        // Arrange
        List<Category> categories = CategoryFixture.GetCategories(2, true);
        await InsertBatchRange(categories);
        Category category1 = categories.First();
        Category categoryToUpdate = categories[1];
        categoryToUpdate.SetName(category1.Name);
        // Act
        HttpResponseMessage httpResponse = await Client.PutAsJsonAsync(
            CategoriesRoutes.Update(categoryToUpdate.Id),
            categoryToUpdate.ToUpdateCommand()
        );
        // Assert
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        ProblemDetails errorResponse = await httpResponse.GetErrorResponse<ProblemDetails>();
        errorResponse.Detail.ShouldBe(CategoryErrors.NameAlreadyExists.Description);
    }

    [Fact]
    public async Task Update_WhenDataIsNotFound_ShouldReturn404WithError()
    {
        // Arrange
        UpdateCategory.Command command = _category.ToUpdateCommand();

        // Act
        HttpResponseMessage httpResponse = await Client.PutAsJsonAsync(
            CategoriesRoutes.Update(command.Id),
            command
        );
        // Assert
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        // Assert
        ProblemDetails errorResponse = await httpResponse.GetErrorResponse<ProblemDetails>();
        errorResponse.Detail.ShouldBe(CategoryErrors.NotFound(command.Id.ToString()).Description);
    }

    [Fact]
    public async Task Update_WhenValidData_ShouldReturn204()
    {
        // Arrange
        await AddAsync(_category);
        UpdateCategory.Command command = _category.ToUpdateCommand();
        // Act
        HttpResponseMessage httpResponse = await Client.PutAsJsonAsync(
            CategoriesRoutes.Update(command.Id),
            command
        );
        // Assert
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
