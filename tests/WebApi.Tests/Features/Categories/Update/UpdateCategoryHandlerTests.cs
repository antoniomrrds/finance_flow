using WebApi.Domain.Categories;
using WebApi.Features.Categories.Update;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.TestBase;

namespace WebApi.Tests.Features.Categories.Update;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(UpdateCategory))]
public class UpdateCategoryHandlerTests : DbContextTestBase
{
    private readonly UpdateCategory.Handler _sut;
    private readonly CancellationToken _ct = CancellationToken.None;

    public UpdateCategoryHandlerTests()
    {
        _sut = new UpdateCategory.Handler(Db);
    }

    [Fact]
    public async Task Handle_WhenDataIsValid_ShouldReturnSuccess()
    {
        // Arrange
        Category expectedCategory = CategoryFixture.GetCategory(true);
        await SeedAsync(expectedCategory);
        // Act
        Result result = await _sut.Handle(expectedCategory.ToUpdateCommand(), _ct);
        // Assert
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Handle_WhenTheCategoryDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        Category expectedCategory = CategoryFixture.GetCategory(true);
        // Act
        Result result = await _sut.Handle(expectedCategory.ToUpdateCommand(), _ct);
        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(CategoryErrors.NotFound(expectedCategory.Id.ToString()));
    }

    [Fact]
    public async Task Handle_WhenTheCategoryNameExists_ShouldReturnFailure()
    {
        // Arrange
        List<Category> categories = CategoryFixture.GetCategories(2, true);
        await SeedRangeAsync(categories);
        Category category1 = categories.First();
        Category categoryToCreate = categories[1];
        categoryToCreate.SetName(category1.Name);
        // Act
        Result result = await _sut.Handle(categoryToCreate.ToUpdateCommand(), _ct);
        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NameAlreadyExists);
    }
}
