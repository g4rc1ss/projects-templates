namespace Shared;

public class Result
{
    public object? Data { get; }
    public bool IsSuccess => Error == Error.none;
    public Error Error { get; }

    public Result()
    {
        Data = null;
        Error = Error.none;
    }

    protected Result(object? data)
        : this()
    {
        Data = data;
    }

    public Result(Error error)
    {
        if (error == Error.none)
        {
            throw new InvalidOperationException("Error cannot be none");
        }

        Error = error;
    }

    public static Result Success() => new();

    public static Result<T> Success<T>(T data) => new(data);

    public static Result Failure(Error error) => new(error);

    public static Result<T> Failure<T>(Error error) => new(error);
}

public class Result<T> : Result
{
    public new T Data => (T?)base.Data ?? throw new NullReferenceException();

    public Result(T? data)
        : base(data)
    {
    }

    public Result(Error error)
        : base(error)
    {
    }

    public static implicit operator Result<T>(T? data) =>
        data is not null ? Success(data) : Failure<T>(Error.nullValue);

    public static implicit operator Result<T>(Error error) => Failure<T>(error);
}

public record Error(string Code, string Message)
{
    public static readonly Error none = new(string.Empty, string.Empty);
    public static readonly Error nullValue = new("Error.NullValue", "Null value was provided.");
}