namespace ResultTools;

public static class ResultExtensions
{
    public static Result<TOutput> Then<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, TOutput> continuation)
        => input.Then(inp => Result.Of(() => continuation(inp)));
    
    public static Result<None> Then<TInput>(
        this Result<TInput> input,
        Action<TInput> continuation)
    => input.Then(inp => Result.OfAction(() => continuation(inp)));

    public static Result<TOutput> Then<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation)
        => input.IsSuccess
            ? continuation(input.Value!)
            : Result.Fail<TOutput>(input.Error!);

    public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> input, Result<TOutput> value)
        => input.Then(_ => value);
    
    public static Result<TInput> OnFail<TInput>(
        this Result<TInput> input,
        Action<string> handleError)
    {
        if (!input.IsSuccess)
            handleError(input.Error!);
        return input;
    }

    public static Result<TInput> ReplaceError<TInput>(
        this Result<TInput> input,
        Func<string, string> replaceError)
        => input.IsSuccess
            ? input
            : Result.Fail<TInput>(replaceError(input.Error!));

    public static Result<TInput> RefineError<TInput>(
        this Result<TInput> input,
        string error)
        => input.ReplaceError(err => $"{error}. {err}");
}