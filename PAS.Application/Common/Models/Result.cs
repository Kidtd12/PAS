namespace Application.Common.Models;

public class Result
{
    public bool Succeeded { get; protected set; }
    public string[] Errors { get; protected set; } = Array.Empty<string>();

    public static Result Success() => new() { Succeeded = true };

    public static Result Failure(params string[] errors) => new()
    {
        Succeeded = false,
        Errors = errors?.Where(e => !string.IsNullOrWhiteSpace(e)).ToArray() ?? Array.Empty<string>()
    };

    public static Result Failure(string error) => Failure(new[] { error });
}

public class Result<T> : Result
{
    public T? Data { get; private set; }

    public static Result<T> Success(T data) => new() { Succeeded = true, Data = data };

    public new static Result<T> Failure(params string[] errors) => new()
    {
        Succeeded = false,
        Errors = errors?.Where(e => !string.IsNullOrWhiteSpace(e)).ToArray() ?? Array.Empty<string>()
    };

    public new static Result<T> Failure(string error) => Failure(new[] { error });
}
