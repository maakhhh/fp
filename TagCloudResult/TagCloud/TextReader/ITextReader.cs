namespace TagCloud.TextReader;

public interface ITextReader
{
    IReadOnlyList<string> SupportedFormats();
    string Read();
}
