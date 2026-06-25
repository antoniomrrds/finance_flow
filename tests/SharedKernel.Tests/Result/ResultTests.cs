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
}
