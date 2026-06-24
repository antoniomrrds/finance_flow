namespace WebApi.Domain.Categories;

internal sealed class Category
{
    public Guid Id { get; init; }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public Category(Guid id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    // Required for Entity Framework core
    private Category() { }
}
