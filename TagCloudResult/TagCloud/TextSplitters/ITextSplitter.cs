namespace TagCloud.TextSplitters;

public interface ITextSplitter
{
    IEnumerable<string> Split(string text);
}
