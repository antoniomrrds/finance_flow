using WebApi.Domain.Categories;
using WebApi.Features.Categories.Create;
using WebApi.Features.Categories.Delete;
using WebApi.Features.Categories.GetById;
using WebApi.Features.Categories.Update;
using WebApi.Infrastructure.Data.Repositories;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.TestBase;

namespace WebApi.Tests.Infrastructure.Data.Repositories;

[Trait("Module", nameof(Category))]
[Trait("Feature", nameof(CreateCategory))]
public class CategoryRepositoryTests : RepositoryTestBase
{
    private readonly CategoryRepository _sut;
    private readonly Category _category;

    public CategoryRepositoryTests()
    {
        _category = CategoryFixture.GetCategory(true);
        _sut = new CategoryRepository(Db);
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
        await AddAsync(_category);
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
        await Db.SaveChangesAsync();
        //Act
        Category? saved = await Db.Categories.FirstOrDefaultAsync(c => c.Id == _category.Id);

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
        await AddAsync(_category);
        // Act
        Category? category = await _sut.GetByIdAsync(_category.Id);
        // Assert
        category.ShouldNotBeNull();
        category.ShouldMatch(_category);
    }

    [Trait("Module", nameof(Category))]
    [Trait("Feature", nameof(UpdateCategory))]
    [Fact]
    public async Task UpdateIfValidAsync_IfTheCategoryDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange && Act
        CategoryUpdateOutcome categoryUpdateOutcome = await _sut.UpdateIfValidAsync(_category);
        // Assert
        categoryUpdateOutcome.ShouldBe(CategoryUpdateOutcome.NotFound);
    }

    [Trait("Module", nameof(Category))]
    [Trait("Feature", nameof(UpdateCategory))]
    [Fact]
    public async Task UpdateIfValidAsync_IfTheCategoryNameExists_ShouldReturnNameConflict()
    {
        // Arrange
        List<Category> categories = CategoryFixture.GetCategories(2, true);
        await InsertBatchRange(categories);
        Category category1 = categories.First();
        Category categoryToUpdate = categories[1];
        categoryToUpdate.SetName(category1.Name);
        // Act
        CategoryUpdateOutcome categoryUpdateOutcome = await _sut.UpdateIfValidAsync(
            categoryToUpdate
        );
        // Assert
        categoryUpdateOutcome.ShouldBe(CategoryUpdateOutcome.NameConflict);
    }

    [Trait("Module", nameof(Category))]
    [Trait("Feature", nameof(UpdateCategory))]
    [Fact]
    public async Task UpdateIfValidAsync_WhenDataIsValid_ShouldReturnUpdated()
    {
        // Arrange
        await AddAsync(_category);
        // Act
        CategoryUpdateOutcome categoryUpdateOutcome = await _sut.UpdateIfValidAsync(_category);
        // Assert
        categoryUpdateOutcome.ShouldBe(CategoryUpdateOutcome.Updated);
    }

    [Trait("Module", nameof(Category))]
    [Trait("Feature", nameof(DeleteCategory))]
    [Fact]
    public async Task DeleteAsync_WhenTheCategoryDoesNotExist_ShouldReturnFalse()
    {
        // Arrange && Act
        bool isDeleted = await _sut.DeleteAsync(_category.Id);
        // Assert
        isDeleted.ShouldBeFalse();
    }

    [Trait("Module", nameof(Category))]
    [Trait("Feature", nameof(DeleteCategory))]
    [Fact]
    public async Task DeleteAsync_WhenTheCategoryExists_ShouldReturnTrue()
    {
        // Arrange
        await AddAsync(_category);
        // Act
        bool isDeleted = await _sut.DeleteAsync(_category.Id);
        // Assert
        isDeleted.ShouldBeTrue();
    }
}
