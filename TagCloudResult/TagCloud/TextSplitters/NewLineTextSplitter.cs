namespace TagCloud.TextSplitters;

public class NewLineTextSplitter : ITextSplitter
{
    private readonly string SPLIT_SYMBOLS = Environment.NewLine;
    public IEnumerable<string> Split(string text)
    {
        ArgumentNullException.ThrowIfNull(text, nameof(text));
        return text.Split(SPLIT_SYMBOLS).Select(w => w.Trim()).Where(w => w != string.Empty);
    }
}
