using SharedKernel;

namespace WebApi.Domain.Categories;

static class CategoryErrorCodes
{
    private const string Prefix = "CATEGORY";
    public const string NotFound = $"{Prefix}.{ErrorPatterns.NotFound}";
    public const string NameAlreadyExists = $"{Prefix}.{ErrorPatterns.AlreadyExists}";
}

public static class CategoryErrors
{
    public static FailureReason NotFound(string id) =>
        new(CategoryErrorCodes.NotFound, $"A categoria com o id: '{id}' não foi encontrada.");

    public static readonly FailureReason NameAlreadyExists = new(
        CategoryErrorCodes.NameAlreadyExists,
        "Uma categoria com o nome fornecido já existe."
    );
}

static class ErrorPatterns
{
    public const string NotFound = "NOT_FOUND";
    public const string AlreadyExists = "ALREADY_EXISTS";
}
