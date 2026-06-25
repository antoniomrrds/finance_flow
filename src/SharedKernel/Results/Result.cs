namespace SharedKernel.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public FailureReason Error { get; }

    Result(bool isSuccess, FailureReason? error = null)
    {
        error ??= FailureReason.None;
        if (isSuccess && error != FailureReason.None || !isSuccess && error == FailureReason.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true);

    public static Result Failure(FailureReason error) => new(false, error);
}
