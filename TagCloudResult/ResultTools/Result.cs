namespace ResultTools;

public class Result<T>
{
    public T? Value { get; }
    public string? Error { get; }
    public bool IsSuccess => Error == null;

    public Result(string? error, T? value = default)
    {
        Error = error;
        Value = value;
    }

    public T GetValueOrThrow()
    {
        if (IsSuccess)
            return Value!;
        throw new InvalidOperationException(Error);
    }
}

public static class Result
{
    public static Result<T> Ok<T>(T value) => new(null, value);

    public static Result<None> Ok() => new(null);
    
    public static Result<T> AsResult<T>(this T value) => Ok(value);
    
    public static Result<T> Fail<T>(string e) => new(e);

    public static Result<T> Of<T>(Func<T> f, string? error = null)
    {
        try
        {
            return Ok(f());
        }
        catch (Exception e)
        {
            return Fail<T>(error ?? e.Message);
        }
    }

    public static Result<None> OfAction(Action f, string? error = null)
    {
        try
        {
            f();
            return Ok();
        }
        catch (Exception e)
        {
            return Fail<None>(error ?? e.Message);
        }
    }
}