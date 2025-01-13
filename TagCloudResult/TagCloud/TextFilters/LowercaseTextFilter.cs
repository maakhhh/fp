
namespace TagCloud.TextFilters;

public class LowercaseTextFilter : ITextFilter
{
    public IEnumerable<string> Apply(IEnumerable<string> words) 
        => words.Select(x => x.ToLower());
}
