namespace Shared;

public class Result
{
    public object? Data { get; }
    public bool IsSuccess { get; }
    public Error Error { get; }

    protected Result(object? data, bool isSuccess, Error error)
        : this(isSuccess, error)
    {
        Data = data;
    }

    private Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.none)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.none)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.none);

    public static Result<T> Success<T>(T data) => new(data, true, Error.none);

    public static Result Failure(Error error) => new(false, error);

    public static Result<T> Failure<T>(Error error) => new(default, false, error);
}

public class Result<T>(T? data, bool isSuccess, Error error) : Result(data, isSuccess, error)
{
    public new T? Data => data;

    public static implicit operator Result<T>(T? data) =>
        data is not null ? Success(data) : Failure<T>(Error.nullValue);

    public static implicit operator Result<T>(Error error) => Failure<T>(error);
}

public record Error(string Code, string Message)
{
    public static readonly Error none = new(string.Empty, string.Empty);
    public static readonly Error nullValue = new("Error.NullValue", "Null value was provided.");
}
