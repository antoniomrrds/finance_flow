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
    private readonly AppDbContext _db;

    public CategoryReaderRepositoryTests()
    {
        _category = CategoryFixture.GetCategory(true);
        _db = TestDbFactory.Create();
        _sut = new CategoryReaderRepository(_db);
    }

    private async Task AddCategory()
    {
        await _db.Categories.AddAsync(_category);
        await _db.SaveChangesAsync();
    }

    [Fact]
    public async Task HasCategoryWithNameAsync_WhenTheNameDoesNotExist_ShouldReturnFalse()
    {
        //Arrange & Act
        bool nameExists = await _sut.HasCategoryWithNameAsync(_category.Name);
        // Assert
        nameExists.ShouldBeFalse();
    }

    [Fact]
    public async Task HasCategoryWithNameAsync_WhenNameAlreadyExists_ShouldReturnTrue()
    {
        // Arrange
        await AddCategory();
        // Act
        bool nameExists = await _sut.HasCategoryWithNameAsync(_category.Name);
        // Assert
        nameExists.ShouldBeTrue();
    }
}
