using FluentValidation;

namespace WebApi.Features.Categories.Common;

public abstract class CommonCategoryValidator<T> : AbstractValidator<T>
    where T : CommonCategoryProperties
{
    protected CommonCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é obrigatório.")
            .Length(4, 150)
            .WithMessage(
                "O campo {PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres."
            );

        RuleFor(x => x.Description)
            .MaximumLength(200)
            .WithMessage("O campo {PropertyName} não pode exceder {MaxLength} caracteres.")
            .Must(x => string.IsNullOrEmpty(x) || x.Trim().Length > 0)
            .WithMessage("O campo {PropertyName} não pode conter apenas espaços em branco.");
    }
}
