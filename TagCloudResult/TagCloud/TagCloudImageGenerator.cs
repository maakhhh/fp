using TagCloud.BitmapGenerators;
using TagCloud.CloudImageSavers;
using TagCloud.TextFilters;
using TagCloud.TextReader;
using TagCloud.TextSplitters;

namespace TagCloud;

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

    public string GenerateCloud()
    {
        var reader = readerProvider.GetActualReader();
        var words = splitter.Split(reader.Read());

        var wordsFrequency = filters
            .Aggregate(words, (word, filter) => filter.Apply(word))
            .GroupBy(w => w)
            .ToDictionary(words => words.Key, words => words.Count());

        var minWordCount = wordsFrequency.Values.Min();
        var maxWordCount = wordsFrequency.Values.Max();

        var cloudWords = wordsFrequency
            .Select(w => new CloudWord(w.Key, GetWordFontSize(
                w.Value, minWordCount, maxWordCount)));

        var bitmap = bitmapGenerator.GenerateBitmapFromWords(cloudWords);
        return saver.Save(bitmap);
    }

    private int GetWordFontSize(int freqCount, int minWordCount, int maxWordCount) =>
        MIN_FONTSIZE + (MAX_FONTSIZE - MIN_FONTSIZE) 
        * (freqCount - minWordCount) / (maxWordCount - minWordCount);
}
