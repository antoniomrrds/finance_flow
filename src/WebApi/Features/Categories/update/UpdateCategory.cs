using FluentValidation;
using WebApi.Configuration;
using WebApi.Configuration.Docs;
using WebApi.Domain.Categories;
using WebApi.Features.Abstractions.Data;
using WebApi.Features.Categories.Shared;
using WebApi.Infrastructure.Http;

namespace WebApi.Features.Categories.Update;

public static class UpdateCategory
{
    public sealed record Command : CommonCategoryProperties, ICommand
    {
        public Guid Id { get; set; }
    };

    public sealed record Request
    {
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
    }

    public class Validator : CommonCategoryValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Informe um {PropertyName} vàlido.");
        }
    }

    public sealed class Endpoint : IEndpoint<CategoryGroup>
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut(
                    "/{id:guid}",
                    async (
                        Guid id,
                        Request request,
                        ICommandHandler<Command> handler,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        Command command = new()
                        {
                            Id = id,
                            Name = request.Name,
                            Description = request.Description,
                        };
                        Result result = await handler.Handle(command, cancellationToken);
                        return result.Match(Results.NoContent, CustomProblemResults.Problem);
                    }
                )
                .WithSummary("Atualizar uma categoria")
                .ProducesNoContent("Categoria atualizada com sucesso.")
                .ProducesValidationProblemWithDescription()
                .ProducesConflict(
                    "Conflito: Já existe uma categoria cadastrada com o nome informado."
                )
                .ProducesNotFound("Categoria não encotrada.")
                .ProducesInternalServerError();
        }
    }

    public sealed class Handler(IApplicationDbContext context) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            Category category = new(
                id: command.Id,
                name: command.Name,
                description: command.Description
            );

            int affectedRows = await context
                .Categories.Where(c => c.Id == category.Id)
                .Where(c =>
                    !context.Categories.Any(other =>
                        other.Id != category.Id && other.Name == category.Name
                    )
                )
                .ExecuteUpdateAsync(
                    setters =>
                        setters
                            .SetProperty(c => c.Name, category.Name)
                            .SetProperty(c => c.Description, category.Description),
                    cancellationToken
                );

            if (affectedRows > 0)
            {
                return Result.Success();
            }

            bool exists = await context.Categories.AnyAsync(
                c => c.Id == category.Id,
                cancellationToken
            );

            return exists
                ? CategoryErrors.NameAlreadyExists
                : CategoryErrors.NotFound(category.Id.ToString());
        }
    }
}
