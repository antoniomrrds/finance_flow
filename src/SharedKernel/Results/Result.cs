namespace SharedKernel.Results;

class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public FailureReason Error { get; }

    Result(bool isSuccess, FailureReason? error = null)
    {
        Error = error ?? FailureReason.None;
        IsSuccess = isSuccess;
    }

    public static Result Success() => new(true);

    public static Result Failure(FailureReason error) => new(false, error);
}
