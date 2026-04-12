namespace Cleanly.Application.Common.Models;

public sealed record Result<T>(bool Success, string? Error, T? Value)
{
    public static Result<T> Ok(T value) => new(true, null, value);
    public static Result<T> Fail(string error) => new(false, error, default);
}
