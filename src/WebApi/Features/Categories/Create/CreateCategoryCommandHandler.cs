using SharedKernel;
using WebApi.Domain.Categories;
using WebApi.Domain.Ports;
using WebApi.Messaging;

namespace WebApi.Features.Categories.Create;

public sealed record CreateCategoryCommand : ICommand<Guid>
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}

internal sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoryRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateCategoryCommandHandler(ICategoryRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _uow = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken
    )
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
