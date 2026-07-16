using WebApi.Domain.Categories;
using WebApi.Features.Categories.GetById;
using WebApi.Features.Categories.Shared;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Features.Categories.Shared;
using WebApi.Tests.Infrastructure.Factories;
using WebApi.Tests.Infrastructure.TestBase;

namespace WebApi.Tests.Features.Categories.GetById;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(GetCategoryById))]
public class GetCategoryByIdEndpointTests : BaseIntegrationTest
{
    private readonly Category _category;

    public GetCategoryByIdEndpointTests(SqliteTestWebAppFactory factory)
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
        content.ShouldMatch(_category.ToResponse<GetCategoryById.Response>());
    }

    [Fact]
    public async Task Update_WhenDataIsNotFound_ShouldReturnFailureAnd404()
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
