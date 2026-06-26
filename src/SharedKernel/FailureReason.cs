namespace SharedKernel;

/// <summary>
/// Representa um motivo de falha com código e descrição
/// </summary>
public record FailureReason(string Code, string Description)
{
    /// <summary>Representa a ausência de erro (sucesso)</summary>
    public static readonly FailureReason None = new(string.Empty, string.Empty);

    /// <summary>Erro para quando um valor nulo é fornecido</summary>
    public static readonly FailureReason NullValue = new(
        "FailureReason.NullValue",
        "Null value was provided"
    );

    /// <summary>Converte implicitamente um FailureReason para um Result de falha</summary>
    public static implicit operator Result(FailureReason error) => Result.Failure(error);
}
