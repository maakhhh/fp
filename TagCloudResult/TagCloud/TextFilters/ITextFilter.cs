using ResultTools;

namespace TagCloud.TextFilters;

public interface ITextFilter
{
    Result<IEnumerable<string>> Apply(IEnumerable<string> text);
}
