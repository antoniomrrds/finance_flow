namespace SharedKernel.Results;

/// <summary>
/// Representa um motivo de falha com código e descrição
/// </summary>
public record FailureReason(string Code, string Description)
{
    public static readonly FailureReason None = new(string.Empty, string.Empty);
}
