using System.Diagnostics.CodeAnalysis;
using ResultTools;
using TagCloud.BitmapGenerators;
using TagCloud.CloudImageSavers;
using TagCloud.TextFilters;
using TagCloud.TextReader;
using TagCloud.TextSplitters;

namespace TagCloud;

[SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
public class TagCloudImageGenerator
{
    private readonly TextReaderProvider readerProvider;
    private readonly ICloudImageSaver saver;
    private readonly IBitmapGenerator bitmapGenerator;
    private readonly ITextSplitter splitter;
    private readonly IEnumerable<ITextFilter> filters;
    private const int MAX_FONTSIZE = 100;
    private const int MIN_FONTSIZE = 8;

    public TagCloudImageGenerator(
        TextReaderProvider readerProvider,
        ITextSplitter splitter,
        ICloudImageSaver saver,
        IBitmapGenerator bitmapGenerator,
        IEnumerable<ITextFilter> filters)
    {
        this.splitter = splitter;
        this.readerProvider = readerProvider;
        this.saver = saver;
        this.bitmapGenerator = bitmapGenerator;
        this.filters = filters;
    }

    public Result<string> GenerateCloud()
    {
        var reader = readerProvider.GetActualReader();
        return reader
            .Then(r => r.Read())
            .Then(text => splitter.Split(text))
            .Then(BuildWordsFrequency)
            .Then(ConvertToCloudWord)
            .Then(bitmapGenerator.GenerateBitmapFromWords)
            .Then(saver.Save);
    }

    private Result<IEnumerable<CloudWord>> ConvertToCloudWord(Dictionary<string, int> wordsFrequency)
    {
        var minWordCount = wordsFrequency.Values.Min();
        var maxWordCount = wordsFrequency.Values.Max();
        
        return wordsFrequency
            .Select(w => new CloudWord(w.Key, GetWordFontSize(
                w.Value, minWordCount, maxWordCount))).AsResult();
    }

    private Result<IEnumerable<string>> ApplyFilters(IEnumerable<string> words) =>
        filters.Aggregate(words.AsResult(), (word, filter) => word.Then(filter.Apply));

    private Result<Dictionary<string, int>> BuildWordsFrequency(IEnumerable<string> words) =>
        ApplyFilters(words).Then(filteredWords => filteredWords
            .GroupBy(w => w)
            .ToDictionary(w => w.Key, w => w.Count()));

    private int GetWordFontSize(int freqCount, int minWordCount, int maxWordCount) =>
        MIN_FONTSIZE + (MAX_FONTSIZE - MIN_FONTSIZE) 
        * (freqCount - minWordCount) / (maxWordCount - minWordCount);
}
