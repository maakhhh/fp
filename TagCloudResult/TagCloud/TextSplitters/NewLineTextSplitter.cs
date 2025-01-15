using ResultTools;

namespace TagCloud.TextSplitters;

public class NewLineTextSplitter : ITextSplitter
{
    private readonly string SPLIT_SYMBOLS = Environment.NewLine;
    public Result<IEnumerable<string>> Split(string? text)
    {
        return text == null
            ? Result.Fail<IEnumerable<string>>($"Argument {nameof(text)} cannot be null")
            : text.Split(SPLIT_SYMBOLS).Select(w => w.Trim()).Where(w => w != string.Empty).AsResult();
    }
}
         