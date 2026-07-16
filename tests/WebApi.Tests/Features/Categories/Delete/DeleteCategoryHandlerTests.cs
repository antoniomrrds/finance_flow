using WebApi.Domain.Categories;
using WebApi.Features.Categories.Delete;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.TestBase;

namespace WebApi.Tests.Features.Categories.Delete;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(DeleteCategory))]
public class DeleteCategoryHandlerTests : DbContextTestBase
{
    private readonly DeleteCategory.Handler _sut;
    private readonly CancellationToken _ct = CancellationToken.None;

    public DeleteCategoryHandlerTests()
    {
        _sut = new DeleteCategory.Handler(Db);
    }

    [Fact]
    public async Task Handle_WhenTheCategoryDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var command = new DeleteCategory.Command(Guid.NewGuid());
        // Act
        Result result = await _sut.Handle(command, _ct);
        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(command.Id.ToString()));
    }

    [Fact]
    public async Task Handle_WhenTheCategoryExists_ShouldReturnSuccessAndDelete()
    {
        // Arrange
        Category category = CategoryFixture.GetCategory(true);
        await SeedAsync(category);
        var command = new DeleteCategory.Command(category.Id);
        // Act
        Result result = await _sut.Handle(command, _ct);
        // Assert
        result.IsSuccess.ShouldBeTrue();
    }
}
