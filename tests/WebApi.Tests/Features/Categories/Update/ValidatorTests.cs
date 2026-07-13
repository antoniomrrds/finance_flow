using WebApi.Domain.Categories;
using WebApi.Features.Categories.update;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Features.Categories.Common;

namespace WebApi.Tests.Features.Categories.Update;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(UpdateCategory))]
public class ValidatorTests
    : CategoryValidatorTestBase<UpdateCategory.Validator, UpdateCategory.Command>
{
    private readonly UpdateCategory.Command _command = CategoryFixture
        .GetCategory(true)
        .ToUpdateCommand();

    protected override UpdateCategory.Command DefaultInput() =>
        new()
        {
            Id = _command.Id,
            Name = _command.Name,
            Description = _command.Description,
        };

    [Fact]
    public void Validator_WhenIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        UpdateCategory.Command input = DefaultInput() with
        {
            Id = Guid.Empty,
        };
        // Act
        TestValidationResult<UpdateCategory.Command>? result = Sut.TestValidate(input);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id).WithErrorMessage("Informe um Id vàlido.");
    }

    [Fact]
    public void Validator_WhenInputIsFullyValid_ShouldPassAllValidations()
    {
        // Arrange
        UpdateCategory.Command input = DefaultInput();
        // Act
        TestValidationResult<UpdateCategory.Command>? result = Sut.TestValidate(input);
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
