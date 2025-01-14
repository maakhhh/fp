
using ResultTools;

namespace TagCloud.TextFilters;

public class LowercaseTextFilter : ITextFilter
{
    public Result<IEnumerable<string>> Apply(IEnumerable<string> words) 
        => words.Select(x => x.ToLower()).AsResult();
}
