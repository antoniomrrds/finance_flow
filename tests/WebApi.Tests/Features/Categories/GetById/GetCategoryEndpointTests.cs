using WebApi.Domain.Categories;
using WebApi.Features.Categories.GetById;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Features.Categories.Common;
using WebApi.Tests.Infrastructure.Factories;
using WebApi.Tests.Infrastructure.Helpers;

namespace WebApi.Tests.Features.Categories.GetById;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(GetCategoryById))]
public class GetCategoryEndpointTests : BaseIntegrationTest
{
    private readonly Category _category;

    public GetCategoryEndpointTests(SqliteTestWebAppFactory factory)
        : base(factory)
    {
        _category = CategoryFixture.GetCategory();
    }

    [Fact]
    public async Task WhenDataExists_ShouldReturnDataAnd200()
    {
        await AddAsync(_category);
        // Arrange
        GetCategoryById.Query query = new(_category.Id);
        // Act
        HttpResponseMessage httpResponse = await Client.GetAsync(
            CategoriesRoutes.GetById(query.Id)
        );
        GetCategoryById.Response content =
            await httpResponse.Content.ReadFromJsonAsync<GetCategoryById.Response>();
        // Assert
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        content.ShouldNotBeNull();
        content.ShouldMatch(_category.ToResponse());
    }

    [Fact]
    public async Task WhenDataIsNotFound_ShouldReturnFailureAnd404()
    {
        // Arrange
        GetCategoryById.Query query = new(_category.Id);
        // Act
        HttpResponseMessage httpResponse = await Client.GetAsync(
            CategoriesRoutes.GetById(query.Id)
        );
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        // Assert
        ProblemDetails errorResponse = await httpResponse.GetErrorResponse<ProblemDetails>();
        errorResponse.Detail.ShouldBe(CategoryErrors.NotFound(query.Id.ToString()).Description);
    }
}
