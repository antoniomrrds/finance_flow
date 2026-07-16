using WebApi.Domain.Categories;
using WebApi.Features.Categories.GetById;
using WebApi.Features.Categories.Shared;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.TestBase;

namespace WebApi.Tests.Features.Categories.GetById;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(GetCategoryById))]
public class GetCategoryByIdQueryHandlerTests : DbContextTestBase
{
    //Method_Condition_Expected

    private readonly GetCategoryById.Handler _sut;
    private readonly Category _category;
    private readonly CancellationToken _ct = CancellationToken.None;

    public GetCategoryByIdQueryHandlerTests()
    {
        _category = CategoryFixture.GetCategory(true);
        _sut = new GetCategoryById.Handler(Db);
    }

    [Fact]
    public async Task Handle_WhenTheCategoryDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var expected = new GetCategoryById.Query(_category.Id);
        // Act
        Result<GetCategoryById.Response> result = await _sut.Handle(expected, _ct);
        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(expected.Id.ToString()));
    }

    [Fact]
    public async Task Handle_WhenTheCategoryExists_ShouldReturnTheSuccessAndData()
    {
        // Arrange
        await SeedAsync(_category);
        var expected = new GetCategoryById.Query(_category.Id);
        // Act
        Result<GetCategoryById.Response> result = await _sut.Handle(expected, _ct);
        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(_category.ToResponse<GetCategoryById.Response>());
    }
}
