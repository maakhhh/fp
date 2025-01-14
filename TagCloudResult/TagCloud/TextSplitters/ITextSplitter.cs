using ResultTools;

namespace TagCloud.TextSplitters;

public interface ITextSplitter
{
    Result<IEnumerable<string>> Split(string text);
}
