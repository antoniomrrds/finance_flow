using WebApi.Domain.Categories;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Features.Categories.Common;

namespace WebApi.Tests.Features.Categories.Create;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(CreateCategory))]
public class CreateCategoryValidatorTests
    : CategoryValidatorTestBase<CreateCategory.CreateCategoryValidator, CreateCategory.Command>
{
    private readonly CreateCategory.Command _command = CategoryFixture
        .GetCategory(true)
        .ToCreateCommand();

    protected override CreateCategory.Command DefaultInput() =>
        new() { Name = _command.Name, Description = _command.Description };

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
