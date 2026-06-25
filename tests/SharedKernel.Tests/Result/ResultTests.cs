using SharedKernel.Results;

namespace SharedKernel.Tests.Result;

[Trait("Type", "Unit")]
public class ResultTests
{
    [Fact]
    public void Success_GivenValidCall_ShouldCreateSuccessfulResult()
    {
        var result = Results.Result.Success();

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();
    }

    [Fact]
    public void Failure_GivenInvalidCall_ShouldCreateFailedResult()
    {
        // Arrange
        var error = new FailureReason("Test.Error", "test.error");
        // Act
        var result = Results.Result.Failure(error);
        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(error);
    }
}
