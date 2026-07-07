using WebApi.Domain.Categories;

namespace WebApi.Tests.Domain.Categories;

[Trait("Module", nameof(Category))]
public class CategoryTests
{
    // Method_GivenScenario_ShouldExpectedResult
    [Fact]
    public void Constructor_GivenValidParameters_ShouldSetProperties()
    {
        // Arrange
        Category expected = CategoryFixture.GetCategory(true);
        // Act
        var sut = new Category(expected.Id, expected.Name, expected.Description);
        // Assert
        sut.Id.ShouldBe(expected.Id);
        sut.Name.ShouldBe(expected.Name);
        sut.Description.ShouldBe(expected.Description);
        sut.CreatedAt.ShouldNotBe(default);
    }

    [Fact]
    public void PrivateConstructor_GivenOnePrivateConstructor_ShouldReturnTrue() =>
        typeof(Category).ShouldHavePrivateConstructor();
}
