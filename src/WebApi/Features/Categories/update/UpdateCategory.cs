using SharedKernel;
using WebApi.Application.Abstractions.Messaging;
using WebApi.Domain.Categories;
using WebApi.Endpoints;
using WebApi.Extensions;
using WebApi.Extensions.Docs;
using WebApi.Features.Categories.Common;
using WebApi.Infrastructure.Http;

namespace WebApi.Features.Categories.update;

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

    public class Validator : CommonCategoryValidator<Command>;

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

    public sealed class Handler(ICategoryRepository repo) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            Category category = new(
                id: command.Id,
                name: command.Name,
                description: command.Description
            );
            CategoryUpdateOutcome result = await repo.UpdateIfValidAsync(
                category,
                cancellationToken
            );

            return result switch
            {
                CategoryUpdateOutcome.NotFound => CategoryErrors.NotFound(category.Id.ToString()),
                CategoryUpdateOutcome.NameConflict => CategoryErrors.NameAlreadyExists,
                _ => Result.Success(),
            };
        }
    }
}
