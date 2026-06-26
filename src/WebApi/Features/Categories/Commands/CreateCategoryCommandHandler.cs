using SharedKernel;
using WebApi.Domain.Categories;
using WebApi.Messaging;

namespace WebApi.Features.Categories.Commands;

public sealed record CreateCategoryCommand : ICommand<Guid>
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}

internal sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    readonly ICategoryWriterRepository _writerRepo;
    readonly ICategoryCheckRepository _checkRepo;

    public CreateCategoryCommandHandler(
        ICategoryWriterRepository writerRepo,
        ICategoryCheckRepository checkRepo
    )
    {
        _writerRepo = writerRepo;
        _checkRepo = checkRepo;
    }

    public async Task<Result<Guid>> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken
    )
    {
        var category = new Category(Guid.NewGuid(), command.Name, command.Description);
        await _checkRepo.HasCategoryWithNameAsync(
            name: category.Name,
            cancellationToken: cancellationToken
        );
        await _writerRepo.AddAsync(category, cancellationToken);
        return category.Id;
    }
}
