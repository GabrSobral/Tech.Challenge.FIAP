namespace Tech.Challenge.Domain.Core;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Exception? Error { get; }

    protected Result(bool isSuccess, Exception? error)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException("Um resultado de sucesso não pode conter um erro.");
        if (!isSuccess && error == null)
            throw new InvalidOperationException("Um resultado de falha precisa conter um erro.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Failure(Exception error) => new Result(false, error);

    public static Result<T> Failure<T>(Exception error) => new Result<T>(false, default!, error);

    public static Result Success() => new Result(true, null);

    public static Result<T> Success<T>(T value) => new Result<T>(true, value, null);
}
