namespace WebApi.Domain.Categories;

public sealed class Category
{
    public Guid Id { get; init; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Category(Guid id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    // Required for Entity Framework core
    private Category() { }
}
