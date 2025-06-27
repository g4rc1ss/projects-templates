using System.Collections.Immutable;

namespace Shared;

public struct Result(ImmutableArray<Error> errors)
{
    public ImmutableArray<Error> Errors { get; } = errors;
    public bool IsSuccess => Errors.IsEmpty;

    public Result() : this([])
    {
    }
}

public struct Result<T>
{
    public readonly T? value;
    private readonly ImmutableArray<Error> _errors;

    public readonly bool IsSuccess => _errors.IsEmpty;

    public Result(T value)
    {
        this.value = value;
        _errors = [];
    }

    public Result(ImmutableArray<Error> errors)
    {
        _errors = errors;
    }

    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(ImmutableArray<Error> errors) => new(errors);
}

public static class ResultExtensions
{
    public static Result<T> ToResult<T>(this T value) => new(value);

    public static Result<T> ToResult<T>(this ImmutableArray<Error> error) => new(error);
}

public record Error(string Code, string Message);