namespace TagCloud.TextFilters;

public interface ITextFilter
{
    IEnumerable<string> Apply(IEnumerable<string> text);
}
