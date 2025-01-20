using ResultTools;
using WeCantSpell.Hunspell;

namespace TagCloud.TextFilters;

public class BoringTextFilter : ITextFilter
{
    private const string ENG_DIC = "./TextFilters/Dictionaries/en.dic";
    private const string ENG_AFF = "./TextFilters/Dictionaries/en.aff";
    
    private readonly Result<WordList> wordList;
    private readonly string[] boringPo = 
        ["po:pronoun", "po:preposition", "po:determiner", "po:conjunction"];

    public BoringTextFilter()
    {
        wordList = File.Exists(ENG_DIC) && File.Exists(ENG_AFF)
            ? WordList.CreateFromFiles(ENG_DIC, ENG_AFF).AsResult()
            : Result.Fail<WordList>("Cannot find WordList dictionaries");
    }

    public Result<IEnumerable<string>> Apply(IEnumerable<string> text) =>
        wordList
            .Then(wordList => text.Where(w => !IsBoring(w, wordList)));

    private bool IsBoring(string word, WordList wordList)
    {
        var details = wordList[word];
        if (details.Length <= 0 || details[0].Morphs.Count <= 0) 
            return false;
        var po = details[0].Morphs[0];
        return boringPo.Contains(po);

    }
}
