using WebApi.Domain.Categories;
using WebApi.Features.Categories.Create;

namespace WebApi.Tests.Domain.Categories;

internal static class CategoryFixture
{
    public static Category GetCategory(bool useNewSeed = false) =>
        BuildFaker(GetSeed(useNewSeed)).Generate();

    public static List<Category> GetCategories(int count, bool useNewSeed = false) =>
        BuildFaker(GetSeed(useNewSeed)).Generate(count);

    private static Faker<Category> BuildFaker(int seed) =>
        FakerHelper.CreateFaker<Category>().UseSeed(seed).CustomInstantiator(CreateCategory);

    private static Category CreateCategory(Faker f)
    {
        string? name = f.Commerce.Department();
        string? description = f.Lorem.Sentence();

        return new Category(Guid.NewGuid(), name, description);
    }

    private static int GetSeed(bool useNewSeed) =>
        useNewSeed ? SecureSeedGenerator.Generate() : FakerConstants.DefaultSeed;
}

internal static class CategoryExtensions
{
    internal static CreateCategory.Command ToCommand(this Category category) =>
        new() { Name = category.Name, Description = category.Description ?? string.Empty };
}
