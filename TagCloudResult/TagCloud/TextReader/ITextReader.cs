using ResultTools;

namespace TagCloud.TextReader;

public interface ITextReader
{
    IReadOnlyList<string> SupportedFormats();
    Result<string> Read();
}
