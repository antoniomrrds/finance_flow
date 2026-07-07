using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TestUtilities.Extensions;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.Factories;
using WebApi.Tests.Infrastructure.Helpers;
using WebApi.Tests.Infrastructure.Http;

namespace WebApi.Tests.Features.Categories.Create;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(CreateCategory))]
public class CreateCategoryEndpointTests : BaseIntegrationTest
{
    private readonly Category _category;

    public CreateCategoryEndpointTests(SqliteTestWebAppFactory factory)
        : base(factory)
    {
        _category = CategoryFixture.GetCategory();
    }

    private async Task AddCategory(Category category)
    {
        await Db.Categories.AddAsync(category);
        await Db.SaveChangesAsync();
    }

    [Fact]
    public async Task WhenValidData_ShouldReturn201()
    {
        // Arrange
        var createCategoryCommand = _category.ToCommand();
        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync(
            "/api/v1/categories",
            createCategoryCommand
        );
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task WhenInValidData_ShouldReturn400WithErrors()
    {
        // Arrange
        var createCategoryCommand = new CreateCategory.Command();
        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync(
            "/api/v1/categories",
            createCategoryCommand
        );
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ProblemDetailsResponse problem = await response.GetErrorResponse<ProblemDetailsResponse>();
        problem.Errors.ShouldNotBeNull();
        problem.Errors.ShouldContainKey(nameof(CreateCategory.Command.Name));
    }

    [Fact]
    public async Task Create_WithExistingName_ShouldReturn409()
    {
        // Arrange
        var createCategoryCommand = _category.ToCommand();
        await AddCategory(_category);
        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync(
            "/api/v1/categories",
            createCategoryCommand
        );
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        ProblemDetails errorResponse = await response.GetErrorResponse<ProblemDetails>();
        errorResponse.Detail.ShouldBe(CategoryErrors.NameAlreadyExists.Description);
    }
}
