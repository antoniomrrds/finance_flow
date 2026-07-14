using WebApi.Domain.Categories;
using WebApi.Features.Categories.Delete;

namespace WebApi.Tests.Features.Categories.Delete;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(DeleteCategory))]
public class Validator
{
    private readonly DeleteCategory.Validator _sut;

    public Validator()
    {
        _sut = new DeleteCategory.Validator();
    }

    [Fact]
    public void Validator_WhenIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteCategory.Command(Guid.Empty);
        // Act
        TestValidationResult<DeleteCategory.Command>? result = _sut.TestValidate(command);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id).WithErrorMessage("Informe um Id vàlido.");
    }
}
