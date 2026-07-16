namespace WebApi.Features.Categories.Shared;

public interface ICategoryResponse
{
    Guid Id { get; init; }
    string Name { get; init; }
    string? Description { get; init; }
    DateTime CreatedAt { get; init; }
}
