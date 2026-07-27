namespace Dokkan.Api.Abstractions;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Result(bool isSuccess,Error error)
    {
        if ((isSuccess && error != Error.None)||(!isSuccess&&error==Error.None))
            throw new InvalidOperationException();

        Error = error;
        IsSuccess = isSuccess;
    }

    public Error Error { get; } = default!;

    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true,Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false,error);

}

public class Result<TValue>:Result
{
    private readonly TValue? _value;
    public Result(TValue? value,bool isSuccess,Error error):base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess ? _value! :
        throw new InvalidOperationException("Failed operation cannot have value");
}