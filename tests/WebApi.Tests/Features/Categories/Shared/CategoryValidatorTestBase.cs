using WebApi.Domain.Categories;
using WebApi.Features.Categories.Create;
using WebApi.Features.Categories.Shared;

namespace WebApi.Tests.Features.Categories.Shared;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(CreateCategory))]
public abstract class CategoryValidatorTestBase<TValidator, TInput>
    where TValidator : IValidator<TInput>, new()
    where TInput : CommonCategoryProperties, new()
{
    protected static TValidator Sut => new();
    protected abstract TInput DefaultInput();

    protected TInput BuildCommand(string? name = null, string? description = null)
    {
        TInput input = DefaultInput();

        return input with
        {
            Name = name ?? input.Name,
            Description = description ?? input.Description,
        };
    }

    [Fact]
    public void Validator_WhenNameIsEmpty_ShouldReturnRequiredError()
    {
        TInput input = BuildCommand(name: string.Empty);

        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        result
            .ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("O campo Name é obrigatório.");
    }

    [Fact]
    public void Validator_WhenNameIsTooShort_ShouldReturnLengthError()
    {
        TInput input = BuildCommand(name: "i");

        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        result
            .ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("O campo Name deve ter entre 4 e 150 caracteres.");
    }

    [Fact]
    public void Validator_WhenNameIsValid_ShouldNotHaveErrors()
    {
        TInput input = BuildCommand();

        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void Validator_WhenDescriptionIsTooLong_ShouldReturnError()
    {
        TInput input = BuildCommand(description: new string('a', 201));

        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        result
            .ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("O campo Description não pode exceder 200 caracteres.");
    }

    [Fact]
    public void Validator_WhenDescriptionIsOnlyWhitespace_ShouldReturnError()
    {
        TInput input = BuildCommand(description: "   ");

        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        result
            .ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("O campo Description não pode conter apenas espaços em branco.");
    }

    [Fact]
    public void Validator_WhenDescriptionIsEmpty_ShouldNotHaveError()
    {
        TInput input = BuildCommand(description: string.Empty);

        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }

    [Fact]
    public void Validator_WhenDescriptionIsValid_ShouldNotHaveError()
    {
        TInput input = BuildCommand();

        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }
}
