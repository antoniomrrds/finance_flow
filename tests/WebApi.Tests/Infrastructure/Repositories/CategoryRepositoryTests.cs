using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Create;
using WebApi.Infrastructure.Data;
using WebApi.Infrastructure.Repositories;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.Factories;

namespace WebApi.Tests.Infrastructure.Repositories;

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

    private async Task AddCategory()
    {
        await _db.Categories.AddAsync(_category);
        await _db.SaveChangesAsync();
    }

    [Fact]
    [Trait("Feature", nameof(CreateCategory))]
    public async Task HasCategoryWithNameAsync_WhenTheNameDoesNotExist_ShouldReturnFalse()
    {
        //Arrange & Act
        bool nameExists = await _sut.HasCategoryWithNameAsync(_category.Name);
        // Assert
        nameExists.ShouldBeFalse();
    }

    [Fact]
    [Trait("Feature", nameof(CreateCategory))]
    public async Task HasCategoryWithNameAsync_WhenNameAlreadyExists_ShouldReturnTrue()
    {
        // Arrange
        await AddCategory();
        // Act
        bool nameExists = await _sut.HasCategoryWithNameAsync(_category.Name);
        // Assert
        nameExists.ShouldBeTrue();
    }

    [Fact]
    [Trait("Feature", nameof(CreateCategory))]
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
}
