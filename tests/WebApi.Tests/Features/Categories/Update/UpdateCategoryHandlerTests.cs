using WebApi.Domain.Categories;
using WebApi.Features.Categories.Update;
using WebApi.Tests.Domain.Categories;

namespace WebApi.Tests.Features.Categories.Update;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(UpdateCategory))]
public class UpdateCategoryHandlerTests
{
    private readonly UpdateCategory.Handler _sut;
    private readonly ICategoryRepository _categoryRepository;
    private readonly CancellationToken _ct = CancellationToken.None;

    public UpdateCategoryHandlerTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _sut = new UpdateCategory.Handler(_categoryRepository);
    }

    [Fact]
    public async Task Handle_WhenDataIsValid_ShouldReturnSuccess()
    {
        // Arrange
        Category expectedCategory = CategoryFixture.GetCategory(true);
        MakeUpdateIfValidAsyncReturns(CategoryUpdateOutcome.Updated);
        // Act
        Result result = await _sut.Handle(expectedCategory.ToUpdateCommand(), _ct);
        await _categoryRepository.Received(1).UpdateIfValidAsync(Arg.Any<Category>(), _ct);
        // Assert
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Handle_WhenTheCategoryDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        Category expectedCategory = CategoryFixture.GetCategory(true);
        MakeUpdateIfValidAsyncReturns(CategoryUpdateOutcome.NotFound);
        // Act
        Result result = await _sut.Handle(expectedCategory.ToUpdateCommand(), _ct);
        await _categoryRepository.Received(1).UpdateIfValidAsync(Arg.Any<Category>(), _ct);
        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(CategoryErrors.NotFound(expectedCategory.Id.ToString()));
    }

    [Fact]
    public async Task Handle_WhenTheCategoryNameExists_ShouldReturnFailure()
    {
        // Arrange
        Category expectedCategory = CategoryFixture.GetCategory(true);
        MakeUpdateIfValidAsyncReturns(CategoryUpdateOutcome.NameConflict);
        // Act
        Result result = await _sut.Handle(expectedCategory.ToUpdateCommand(), _ct);
        await _categoryRepository.Received(1).UpdateIfValidAsync(Arg.Any<Category>(), _ct);
        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NameAlreadyExists);
    }

    private void MakeUpdateIfValidAsyncReturns(CategoryUpdateOutcome returnValue)
    {
        _categoryRepository.UpdateIfValidAsync(Arg.Any<Category>(), _ct).Returns(returnValue);
    }
}
