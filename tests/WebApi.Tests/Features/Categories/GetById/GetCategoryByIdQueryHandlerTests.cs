using WebApi.Domain.Categories;
using WebApi.Features.Categories.GetById;
using WebApi.Tests.Domain.Categories;

namespace WebApi.Tests.Features.Categories.GetById;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(GetCategoryById))]
public class GetCategoryByIdQueryHandlerTests
{
    //Method_Condition_Expected

    private readonly GetCategoryById.Handler _sut;
    private readonly ICategoryRepository _repo;
    private readonly Category _category;
    private readonly CancellationToken _ct = CancellationToken.None;

    public GetCategoryByIdQueryHandlerTests()
    {
        _category = CategoryFixture.GetCategory(true);
        _repo = Substitute.For<ICategoryRepository>();
        _sut = new GetCategoryById.Handler(_repo);
    }

    [Fact]
    public async Task Handle_WhenTheCategoryDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        MakeGetCategoryByIdAsyncReturns(null);
        var expected = new GetCategoryById.Query(_category.Id);
        // Act
        Result<GetCategoryById.CategoryResponse> result = await _sut.Handle(expected, _ct);
        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(expected.CategoryId.ToString()));
    }

    [Fact]
    public async Task Handle_WhenTheCategoryExists_ShouldReturnTheSuccessAndData()
    {
        // Arrange
        MakeGetCategoryByIdAsyncReturns(_category);
        var expected = new GetCategoryById.Query(_category.Id);
        // Act
        Result<GetCategoryById.CategoryResponse> result = await _sut.Handle(expected, _ct);
        await _repo.Received(1).GetByIdAsync(expected.CategoryId, _ct);
        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(_category.ToResponse());
    }

    private void MakeGetCategoryByIdAsyncReturns(Category? returnValue)
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), _ct).Returns(returnValue);
    }
}
