namespace Application.Core;

public class Result<T>
{
    public bool IsSuccess { get; init; }
    
    public T? Value { get; init; }
    
    public string? Error { get; init; }
    
    public int Code { get; init; }
    
    public static Result<T> Success(T value) => new()
    {
        IsSuccess = true,
        Value = value,
        Code = 200
    };
    
    public static Result<T> Failure(string error, int code) => new()
    {
        IsSuccess = false,
        Error = error,
        Code = code
    };
}