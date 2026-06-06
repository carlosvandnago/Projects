namespace TaxRacm.SharedKernel.Domain;

/// <summary>Discriminated union representing either success or failure. Never throw for business rule violations.</summary>
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public bool IsFailure => !IsSuccess;

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    /// <summary>Creates a successful result wrapping the given value.</summary>
    public static Result<T> Success(T value) => new(true, value, null);

    /// <summary>Creates a failure result with the given error message.</summary>
    public static Result<T> Failure(string error) => new(false, default, error);
}

/// <summary>Non-generic result for commands that return no value.</summary>
public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public bool IsFailure => !IsSuccess;

    private Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
}
