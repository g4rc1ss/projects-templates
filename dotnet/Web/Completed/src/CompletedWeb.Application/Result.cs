using System.Collections.Immutable;

namespace CompletedWeb.Application;

public struct Result
{
    public ImmutableArray<Error> Errors { get; }
    public bool IsSuccess => Errors.IsEmpty;

    public Result()
    {
        Errors = [];
    }

    public Result(ImmutableArray<Error> errors)
    {
        if (errors.IsEmpty)
        {
            throw new ArgumentException("errors can't be empty");
        }

        Errors = errors;
    }

    public static Result Failure(ImmutableArray<Error> errors) => new(errors);
    public static Result Failure(Error error) => new([error]);
    public static Result Success() => new();
}

public struct Result<T>
{
    public readonly T? value;
    public ImmutableArray<Error> Errors { get; }

    public readonly bool IsSuccess => Errors.IsEmpty;

    public Result(T value)
    {
        this.value = value;
        Errors = [];
    }

    public Result(ImmutableArray<Error> errors)
    {
        if (errors.IsEmpty)
        {
            throw new ArgumentException("errors can't be empty");
        }

        Errors = errors;
    }
    
    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(ImmutableArray<Error> errors) => new(errors);
}

public static class ResultExtensions
{
    public static Result<T> ToResult<T>(this T value) => new(value);
    public static Result<T> ToResult<T>(this ImmutableArray<Error> error) => new(error);
    public static Result ToResult(this ImmutableArray<Error> error) => new(error);
    public static Result ToResult(this Error error) => new([error]);
}

public record Error(string Code, string Message);