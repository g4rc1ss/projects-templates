namespace Shared;

public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }

    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, Error.None);
    public static Result<T> Success<T>(T data) => new(data, true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);
    public static Result<T> Failure<T>(Error error) => new(default, false, error);

}

public class Result<T> : Result
{
    private readonly T _data;

    public Result(T data, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _data = data;
    }

    public static implicit operator Result<T>(T? data) =>
        data is not null ? Success(data) : Failure<T>(Error.NullValue);
}

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided.");
}