namespace WebApi.Domain.Categories;

internal sealed class Category
{
    public Guid Id { get; init; }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public Category(Guid id, string? description)
    {
        Id = id;
        Description = description;
    }
}
