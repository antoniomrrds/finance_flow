using FluentValidation.TestHelper;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Features.Categories.Common;

namespace WebApi.Tests.Features.Categories.Create;

[Trait("Feature", nameof(CreateCategory))]
public class CreateCategoryValidatorTests
    : CategoryValidatorTestBase<CreateCategory.CreateCategoryValidator, CreateCategory.Command>
{
    private readonly CreateCategory.Command _createCategoryCommand = CategoryFixture
        .GetCategory()
        .ToCommand();

    protected override CreateCategory.Command DefaultInput() =>
        new()
        {
            Name = _createCategoryCommand.Name,
            Description = _createCategoryCommand.Description,
        };

    [Fact]
    public void Validator_WhenInputIsFullyValid_ShouldPassAllValidations()
    {
        // Arrange
        CreateCategory.Command input = DefaultInput();
        // Act
        TestValidationResult<CreateCategory.Command>? result = Sut.TestValidate(input);
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
