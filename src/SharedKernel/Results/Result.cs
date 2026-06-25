using System.Diagnostics.CodeAnalysis;

namespace SharedKernel.Results;

/// <summary>
/// Representa o resultado de uma operação que pode ser bem-sucedida ou falha
/// </summary>
public class Result
{
    protected internal Result(bool isSuccess, FailureReason? error = null)
    {
        error ??= FailureReason.None;

        // Garante consistência: sucesso não pode ter erro, falha deve ter erro
        if (isSuccess && error != FailureReason.None || !isSuccess && error == FailureReason.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>Indica se a operação foi bem-sucedida</summary>
    public bool IsSuccess { get; }

    /// <summary>Indica se a operação falhou</summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>O motivo da falha (se houver)</summary>
    public FailureReason Error { get; }

    /// <summary>Cria um Result de sucesso sem valor</summary>
    public static Result Success() => new(true, FailureReason.None);

    /// <summary>Cria um Result de sucesso com valor</summary>
    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, FailureReason.None);

    /// <summary>Cria um Result de falha sem valor</summary>
    public static Result Failure(FailureReason error) => new(false, error);

    /// <summary>Cria um Result de falha com valor</summary>
    public static Result<TValue> Failure<TValue>(FailureReason error) => new(default, false, error);
}

/// <summary>
/// Versão genérica do Result que carrega um valor
/// </summary>
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, FailureReason? error = null)
        : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// O valor do Result. Só pode ser acessado se for sucesso.
    /// Lança InvalidOperationException se for falha.
    /// </summary>
    [NotNull]
    public TValue Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                "The value of a failure result can't be accessed."
            );

    /// <summary>Converte implicitamente um valor para um Result de sucesso</summary>
    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(FailureReason.NullValue);

    /// <summary>Converte implicitamente um erro para um Result de falha</summary>
    public static implicit operator Result<TValue>(FailureReason error) => Failure<TValue>(error);
}
