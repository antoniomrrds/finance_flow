using SharedKernel;
using WebApi.Domain.Categories;
using WebApi.Domain.Ports;
using WebApi.Messaging;

namespace WebApi.Features.Categories.Commands;

public sealed record CreateCategoryCommand : ICommand<Guid>
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}

internal sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoryWriterRepository _writerRepo;
    private readonly ICategoryCheckRepository _checkRepo;
    private readonly IUnitOfWork _uow;

    public CreateCategoryCommandHandler(
        ICategoryWriterRepository writerRepo,
        ICategoryCheckRepository checkRepo,
        IUnitOfWork unitOfWork
    )
    {
        _writerRepo = writerRepo;
        _checkRepo = checkRepo;
        _uow = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken
    )
    {
        var category = new Category(Guid.NewGuid(), command.Name, command.Description);
        bool hasCategory = await _checkRepo.HasCategoryWithNameAsync(
            name: category.Name,
            cancellationToken: cancellationToken
        );

        if (hasCategory)
            return CategoryErrors.NameAlreadyExists;

        await _writerRepo.AddAsync(category, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);
        return category.Id;
    }
}
