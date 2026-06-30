using WebApi.Domain.Categories;
using WebApi.Infrastructure.Data;
using WebApi.Infrastructure.Repositories;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.Factories;

namespace WebApi.Tests.Infrastructure.Repositories;

[Trait("Integration", "category")]
public class CategoryReaderRepositoryTests
{
    private readonly CategoryReaderRepository _sut;
    private readonly Category _category;

    public CategoryReaderRepositoryTests()
    {
        _category = CategoryFixture.GetCategory(true);
        AppDbContext db = TestDbFactory.Create();
        _sut = new CategoryReaderRepository(db);
    }

    [Fact]
    public async Task HasCategoryWithNameAsync_WhenTheNameDoesNotExist_ShouldReturnFalse()
    {
        //Arrange & Act
        bool nameExist = await _sut.HasCategoryWithNameAsync(_category.Name);
        // Assert
        nameExist.ShouldBeFalse();
    }
}
