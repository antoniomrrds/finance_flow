using SharedKernel;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Domain.Categories;

namespace WebApi.Tests.Features.Categories.Create;

[Trait("Feature", nameof(CreateCategory))]
public class CreateCategoryHandlerTests
{
    private readonly CreateCategory.CreateCategoryHandler _sut;
    private readonly Category _category;
    private readonly CreateCategory.Command _command;
    private readonly ICategoryRepository _repo;
    private readonly CancellationToken _ct = CancellationToken.None;
    private readonly IUnitOfWork _uow;

    public CreateCategoryHandlerTests()
    {
        _category = CategoryFixture.GetCategory();
        _command = CategoryFixture.GetCategory().ToCommand();
        _repo = Substitute.For<ICategoryRepository>();
        _uow = Substitute.For<IUnitOfWork>();

        _sut = new CreateCategory.CreateCategoryHandler(_repo, _uow);
    }

    //Method_Condition_Expected
    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ReturnsSuccessResultWithGuid()
    {
        // Arrange
        MakeHasCategoryWithNameAsyncReturns(false);
        // Act
        Result<Guid> result = await _sut.Handle(_command, _ct);
        // Assert
        await _repo.Received(1).HasCategoryWithNameAsync(_command.Name, _ct);
        await _repo.Received(1).AddAsync(Arg.Any<Category>(), _ct);
        await _uow.Received(1).SaveChangesAsync(_ct);
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_WhenCategoryNameAlreadyExists_ReturnsFailure()
    {
        // Arrange
        MakeHasCategoryWithNameAsyncReturns(true);
        // Act
        Result<Guid> result = await _sut.Handle(_command, _ct);
        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NameAlreadyExists);
    }

    private void MakeHasCategoryWithNameAsyncReturns(bool returnValue)
    {
        _repo.HasCategoryWithNameAsync(_category.Name, _ct).Returns(returnValue);
    }
}
