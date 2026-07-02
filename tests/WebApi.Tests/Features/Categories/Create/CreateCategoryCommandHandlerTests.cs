using SharedKernel;
using WebApi.Domain.Categories;
using WebApi.Domain.Ports;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Domain.Categories;

namespace WebApi.Tests.Features.Categories.Create;

[Trait("Unit", "Category")]
public class CreateCategoryCommandHandlerTests
{
    private readonly CreateCategoryCommandHandler _sut;
    private readonly Category _category;
    private readonly ICategoryRepository _repo;
    private readonly CancellationToken _ct = CancellationToken.None;
    private readonly IUnitOfWork _uow;

    public CreateCategoryCommandHandlerTests()
    {
        _category = CategoryFixture.GetCategory();
        _repo = Substitute.For<ICategoryRepository>();
        _uow = Substitute.For<IUnitOfWork>();

        _sut = new CreateCategoryCommandHandler(_repo, _uow);
    }

    //Method_Condition_Expected
    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ReturnsSuccessResultWithGuid()
    {
        // Arrange
        MakeHasCategoryWithNameAsyncReturns(false);
        CreateCategoryCommand command = _category.ToCommand();
        // Act
        Result<Guid> result = await _sut.Handle(command, _ct);
        // Assert
        await _repo.Received(1).HasCategoryWithNameAsync(command.Name, _ct);
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
        CreateCategoryCommand command = _category.ToCommand();
        // Act
        Result<Guid> result = await _sut.Handle(command, _ct);
        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NameAlreadyExists);
    }

    private void MakeHasCategoryWithNameAsyncReturns(bool returnValue)
    {
        _repo.HasCategoryWithNameAsync(_category.Name, _ct).Returns(returnValue);
    }
}
