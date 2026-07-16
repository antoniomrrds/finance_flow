using WebApi.Domain.Categories;
using WebApi.Features.Categories.Delete;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Features.Categories.Shared;
using WebApi.Tests.Infrastructure.Factories;
using WebApi.Tests.Infrastructure.Http;
using WebApi.Tests.Infrastructure.TestBase;

namespace WebApi.Tests.Features.Categories.Delete;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(DeleteCategory))]
public class DeleteCategoryEndpointTests(SqliteTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private readonly Category _category = CategoryFixture.GetCategory(true);

    [Fact]
    public async Task Delete_WhenGuidIsEmpty_ShouldReturn400WithError()
    {
        //Arrange
        Guid invalidGuid = Guid.Empty;
        Uri requestUri = new($"{CategoriesRoutes.BasePath}/{invalidGuid}", UriKind.Relative);
        //Act
        HttpResponseMessage httpResponse = await Client.DeleteAsync(requestUri);
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        // Assert
        ProblemDetailsResponse problem =
            await httpResponse.GetErrorResponse<ProblemDetailsResponse>();
        problem.Errors.ShouldNotBeNull();
        string[] error = problem.Errors.First().Value;
        error.First().ShouldBe("Informe um Id vàlido.");
    }

    [Fact]
    public async Task Delete_WhenDataIsNotFound_ShouldReturn404WithError()
    {
        // Arrange
        var expectedGuid = Guid.NewGuid();

        // Act
        HttpResponseMessage httpResponse = await Client.DeleteAsync(
            CategoriesRoutes.Delete(expectedGuid)
        );
        // Assert
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        // Assert
        ProblemDetails errorResponse = await httpResponse.GetErrorResponse<ProblemDetails>();
        errorResponse.Detail.ShouldBe(CategoryErrors.NotFound(expectedGuid.ToString()).Description);
    }

    [Fact]
    public async Task Delete_WhenTheIdIsValid_ShouldReturn204()
    {
        //Arrange
        await AddAsync(_category);
        //Act
        HttpResponseMessage response = await Client.DeleteAsync(
            CategoriesRoutes.Delete(_category.Id)
        );
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
