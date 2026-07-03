using SharedKernel;
using WebApi.Application.Abstractions.Messaging;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Categories;
using WebApi.Endpoints;
using WebApi.Extensions;
using WebApi.Features.Categories.Common;
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

                    return result.Match(Results.Ok, CustomProblemResults.Problem);
                }
            );
        }
    }

    internal class CreateCategoryHandler : ICommandHandler<Command, Guid>
    {
        private readonly ICategoryRepository _repo;
        private readonly IUnitOfWork _uow;

        public CreateCategoryHandler(ICategoryRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _uow = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var category = new Category(Guid.NewGuid(), command.Name, command.Description);
            bool hasCategory = await _repo.HasCategoryWithNameAsync(
                name: category.Name,
                cancellationToken: cancellationToken
            );

            if (hasCategory)
                return CategoryErrors.NameAlreadyExists;

            await _repo.AddAsync(category, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);
            return category.Id;
        }
    }
}
