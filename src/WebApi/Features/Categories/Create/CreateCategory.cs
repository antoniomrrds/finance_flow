using WebApi.Configuration;
using WebApi.Configuration.Docs;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Categories;
using WebApi.Features.Abstractions.Data;
using WebApi.Features.Categories.Shared;
using WebApi.Infrastructure.Http;

namespace WebApi.Features.Categories.Create;

public static class CreateCategory
{
    public sealed record Command : CommonCategoryProperties, ICommand<Guid>;

    public class CreateCategoryValidator : CommonCategoryValidator<Command>;

    public sealed class Endpoint : IEndpoint<CategoryGroup>
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost(
                    "/",
                    async (
                        Command command,
                        ICommandHandler<Command, Guid> handler,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        Result<Guid> result = await handler.Handle(command, cancellationToken);
                        return result.Match(
                            onSuccess: _ =>
                                Results.Created($"/categories/{result.Value}", result.Value),
                            onFailure: CustomProblemResults.Problem
                        );
                    }
                )
                .WithSummary("Criar categoria")
                .ProducesCreated<Guid>("Categoria criada com sucesso.")
                .ProducesValidationProblemWithDescription()
                .ProducesConflict(
                    "Conflito: Já existe uma categoria cadastrada com o nome informado."
                )
                .ProducesInternalServerError();
        }
    }

    internal class CreateCategoryHandler(IApplicationDbContext context)
        : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var category = new Category(Guid.NewGuid(), command.Name, command.Description);
            bool hasCategory = await context
                .Categories.Where(c => c.Name == category.Name)
                .AnyAsync(cancellationToken);
            if (hasCategory)
            {
                return CategoryErrors.NameAlreadyExists;
            }

            await context.Categories.AddAsync(category, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
            return category.Id;
        }
    }
}
