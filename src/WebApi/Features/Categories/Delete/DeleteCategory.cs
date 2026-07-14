using FluentValidation;
using SharedKernel;
using WebApi.Application.Abstractions.Messaging;
using WebApi.Domain.Categories;
using WebApi.Endpoints;
using WebApi.Extensions;
using WebApi.Extensions.Docs;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.update;
using WebApi.Infrastructure.Http;

namespace WebApi.Features.Categories.Delete;

public static class DeleteCategory
{
    public sealed record Command(Guid Id) : ICommand;

    public class Validator : AbstractValidator<Command>
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
            app.MapDelete(
                    "/{id:guid}",
                    async (
                        Guid id,
                        ICommandHandler<Command> handler,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        Command command = new(id);
                        Result result = await handler.Handle(command, cancellationToken);
                        return result.Match(Results.NoContent, CustomProblemResults.Problem);
                    }
                )
                .WithSummary("Delete uma categoria")
                .ProducesNoContent("Categoria Deletada com sucesso.")
                .ProducesValidationProblemWithDescription()
                .ProducesNotFound("Categoria não encotrada.")
                .ProducesInternalServerError();
        }
    }

    public sealed class Handler(ICategoryRepository categoryRepository) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            bool result = await categoryRepository.DeleteAsync(command.Id, cancellationToken);
            return result ? Result.Success() : CategoryErrors.NotFound(command.Id.ToString());
        }
    }
}
