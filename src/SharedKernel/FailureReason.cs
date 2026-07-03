namespace SharedKernel;

/// <summary>
/// Representa um motivo de falha com código e descrição
/// </summary>
public record FailureReason
{
    /// <summary>Representa a ausência de erro (sucesso)</summary>
    public static readonly FailureReason None = new(string.Empty, string.Empty, ErrorType.Failure);

    /// <summary>Erro para quando um valor nulo é fornecido</summary>
    public static readonly FailureReason NullValue = new(
        "FailureReason.NullValue",
        "Null value was provided",
        ErrorType.Failure
    );

    public string Code { get; }
    public string Description { get; }
    public ErrorType Type { get; }

    public FailureReason(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public static FailureReason Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static FailureReason NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static FailureReason Problem(string code, string description) =>
        new(code, description, ErrorType.Problem);

    public static FailureReason Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    /// <summary>Converte implicitamente um FailureReason para um Result de falha</summary>
    public static implicit operator Result(FailureReason error) => Result.Failure(error);
}

/// <summary>
/// Tipos de erro para categorização
/// </summary>
public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    Problem = 2,
    NotFound = 3,
    Conflict = 4,
}
