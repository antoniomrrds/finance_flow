namespace SharedKernel;

public sealed record ValidationError : FailureReason
{
    public ValidationError(FailureReason[] errors)
        : base("Validation.General", "One or more validation errors occurred", ErrorType.Validation)
    {
        Errors = errors;
    }

    public FailureReason[] Errors { get; }

    public static ValidationError FromResults(IEnumerable<Result> results) =>
        new(results.Where(r => r.IsFailure).Select(r => r.Error).ToArray());
}
