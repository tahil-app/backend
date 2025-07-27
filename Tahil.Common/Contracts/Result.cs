namespace Tahil.Common.Contracts;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; } = string.Empty;
    public List<string> ValidationErrors { get; } = new();

    private Result(bool isSuccess, T value, string? error, List<string>? validationErrors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error ?? string.Empty;
        ValidationErrors = validationErrors ?? new();
    }

    public static Result<T> Success(T value) => new(true, value, null, null);
    public static Result<T> Failure(string error) => new(false, default!, error, null);
    public static Result<T> Failure(T error, string? message) => new(false, error, message, null);
    public static Result<T> Invalid(List<string> validationErrors) => new(false, default!, "Validation failed", validationErrors);

    public Result<TNew> Map<TNew>(Func<T, TNew> mapper)
    {
        return IsSuccess
            ? Result<TNew>.Success(mapper(Value))
            : Result<TNew>.Failure(Error);
    }

    public async Task<Result<TNew>> MapAsync<TNew>(Func<T, Task<TNew>> mapper)
    {
        return IsSuccess
            ? Result<TNew>.Success(await mapper(Value))
            : Result<TNew>.Failure(Error);
    }
}

public static class Result
{
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);
    public static Result<T> Failure<T>(string error) => Result<T>.Failure(error);
    public static Result<T> Failure<T>(IEnumerable<string> errors) => Result<T>.Invalid(errors.ToList());
}