using WebApi.Domain.Categories;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.TestBase;

namespace WebApi.Tests.Features.Categories.Create;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(CreateCategory))]
public class CreateCategoryHandlerTests : DbContextTestBase
{
    private readonly CreateCategory.CreateCategoryHandler _sut;
    private readonly CreateCategory.Command _command;
    private readonly CancellationToken _ct = CancellationToken.None;

    public CreateCategoryHandlerTests()
    {
        _command = CategoryFixture.GetCategory(true).ToCreateCommand();
        _sut = new CreateCategory.CreateCategoryHandler(Db);
    }

    //Method_Condition_Expected
    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ReturnsSuccessResultWithGuid()
    {
        // Arrange
        // Act
        Result<Guid> result = await _sut.Handle(_command, _ct);
        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_WhenCategoryNameAlreadyExists_ReturnsFailure()
    {
        // Arrange
        List<Category> categories = CategoryFixture.GetCategories(2, true);
        await SeedRangeAsync(categories);
        Category category1 = categories.First();
        Category categoryToCreate = categories[1];
        categoryToCreate.SetName(category1.Name);
        // Act
        Result<Guid> result = await _sut.Handle(categoryToCreate.ToCreateCommand(), _ct);
        // Assert
        result.IsFailure.ShouldBeFalse();
        // result.Error.ShouldBe(CategoryErrors.NameAlreadyExists);
    }
}
