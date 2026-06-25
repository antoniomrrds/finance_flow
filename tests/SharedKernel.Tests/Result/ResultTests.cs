using SharedKernel.Results;

namespace SharedKernel.Tests.Result;

[Trait("Type", "Unit")]
public class ResultTests
{
    //Method_Condition_Expected
    [Fact]
    public void Success_WithValidCall_ReturnsSuccessResult()
    {
        var result = Results.Result.Success();

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();
    }

    [Fact]
    public void Failure_WithValidError_ReturnsFailedResult()
    {
        // Arrange
        var error = new FailureReason("Test.Error", "Test error");
        // Act
        var result = Results.Result.Failure(error);
        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void Failure_WithNoneError_ThrowsArgumentException()
    {
        // Arrange
        FailureReason error = FailureReason.None;

        // Act & Assert
        Should.Throw<ArgumentException>(() => Results.Result.Failure(error));
    }

    [Fact]
    public void Success_WithValue_ShouldReturnValue()
    {
        //Arrange
        string expected = "Hello World";
        // Act
        var result = Results.Result.Success(expected);
        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expected);
    }

    [Fact]
    public void ImplicitConversion_FromValue_ShouldCreateSuccess()
    {
        // Arrange
        const string value = "test";
        // Act
        Result<string> result = value;
        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(value);
    }

    [Fact]
    public void ImplicitConversion_FromNull_ShouldCreateFailure()
    {
        // Arrange
        string? value = null;

        // Act
        Result<string> result = value!;

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(FailureReason.NullValue);
    }

    [Fact]
    public void ImplicitConversion_FromFailureReason_ShouldCreateFailure()
    {
        // Arrange
        var error = new FailureReason("Test.Error", "Test error");

        // Act
        Result<string> result = error;

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(error);
    }
}
