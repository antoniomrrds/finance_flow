namespace SharedKernel;

public sealed record ValidationFieldError(string Property, string Code, string Description);

public sealed record ValidationError : FailureReason
{
    public ValidationError(ValidationFieldError[] errors)
        : base("Validation.General", "One or more validation errors occurred", ErrorType.Validation)
    {
        Errors = errors;
    }

    public ValidationFieldError[] Errors { get; }
}
