using WebApi.Domain.Categories;
using WebApi.Features.Categories.Create;
using WebApi.Features.Categories.GetById;
using WebApi.Infrastructure.Persistence.Data;
using WebApi.Infrastructure.Persistence.Repositories;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.Factories;

namespace WebApi.Tests.Infrastructure.Repositories;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(CreateCategory))]
public class CategoryRepositoryTests
{
    private readonly CategoryRepository _sut;
    private readonly Category _category;
    private readonly AppDbContext _db;

    public CategoryRepositoryTests()
    {
        _category = CategoryFixture.GetCategory(true);
        _db = TestDbFactory.Create();
        _sut = new CategoryRepository(_db);
    }

    private async Task AddCategory(Category category)
    {
        await _db.Categories.AddAsync(category);
        await _db.SaveChangesAsync();
    }

    [Trait("Module", nameof(Category))]
    [Trait("Feature", nameof(CreateCategory))]
    [Fact]
    public async Task HasCategoryWithNameAsync_WhenTheNameDoesNotExist_ShouldReturnFalse()
    {
        //Arrange & Act
        bool nameExists = await _sut.HasCategoryWithNameAsync(_category.Name);
        // Assert
        nameExists.ShouldBeFalse();
    }

    [Trait("Module", nameof(Category))]
    [Trait("Feature", nameof(CreateCategory))]
    [Fact]
    public async Task HasCategoryWithNameAsync_WhenNameAlreadyExists_ShouldReturnTrue()
    {
        // Arrange
        await AddCategory(_category);
        // Act
        bool nameExists = await _sut.HasCategoryWithNameAsync(_category.Name);
        // Assert
        nameExists.ShouldBeTrue();
    }

    [Trait("Module", nameof(Category))]
    [Trait("Feature", nameof(CreateCategory))]
    [Fact]
    public async Task AddAsync_WhenCategoryIsValid_ShouldPersistCategory()
    {
        //Arrange
        await _sut.AddAsync(_category);
        await _db.SaveChangesAsync();
        //Act
        Category? saved = await _db.Categories.FirstOrDefaultAsync(c => c.Id == _category.Id);

        // Assert
        saved.ShouldNotBeNull();
        saved.Name.ShouldBe(_category.Name);
        saved.Id.ShouldNotBe(Guid.Empty);
    }

    [Trait("Module", nameof(Category))]
    [Trait("Feature", nameof(GetCategoryById))]
    [Fact]
    public async Task GetByIdAsync_WhenCategoryDoesNotExist_ShouldReturnNull()
    {
        // Arrange & Act
        Category? category = await _sut.GetByIdAsync(_category.Id);
        // Assert
        category.ShouldBeNull();
    }

    [Trait("Module", nameof(Category))]
    [Trait("Feature", nameof(GetCategoryById))]
    [Fact]
    public async Task GetByIdAsync_WhenTheCategoryExists_ShouldReturnTheData()
    {
        // Arrange
        await AddCategory(_category);
        // Act
        Category? category = await _sut.GetByIdAsync(_category.Id);
        // Assert
        category.ShouldNotBeNull();
        category.ShouldMatch(_category);
    }
}
