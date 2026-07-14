using WebApi.Domain.Categories;
using WebApi.Features.Categories.Delete;

namespace WebApi.Tests.Features.Categories.Delete;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(DeleteCategory))]
public class DeleteCategoryHandlerTests
{
    private readonly DeleteCategory.Handler _sut;
    private readonly ICategoryRepository _categoryRepository;
    private readonly CancellationToken _ct = CancellationToken.None;

    public DeleteCategoryHandlerTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _sut = new DeleteCategory.Handler(_categoryRepository);
    }

    [Fact]
    public async Task Handle_WhenTheCategoryDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var command = new DeleteCategory.Command(Guid.NewGuid());
        MakeDeleteCategoryReturns(false);
        // Act
        Result result = await _sut.Handle(command, _ct);
        await _categoryRepository.Received(1).DeleteAsync(Arg.Any<Guid>(), _ct);
        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(command.Id.ToString()));
    }

    [Fact]
    public async Task Handle_WhenTheCategoryExists_ShouldReturnSuccessAndDelete()
    {
        // Arrange
        var command = new DeleteCategory.Command(Guid.NewGuid());
        MakeDeleteCategoryReturns();
        // Act
        Result result = await _sut.Handle(command, _ct);
        await _categoryRepository.Received(1).DeleteAsync(Arg.Any<Guid>(), _ct);
        // Assert
        result.IsSuccess.ShouldBeTrue();
    }

    private void MakeDeleteCategoryReturns(bool returnValue = true)
    {
        _categoryRepository.DeleteAsync(Arg.Any<Guid>(), _ct).Returns(returnValue);
    }
}
