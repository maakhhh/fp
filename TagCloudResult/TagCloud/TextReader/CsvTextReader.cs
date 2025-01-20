using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using ResultTools;
using TagCloud.SettingsProviders;

namespace TagCloud.TextReader;

public class CsvTextReader : ITextReader
{
    private readonly ISettingsProvider<TextReaderSettings> settingsProvider;

    public CsvTextReader(ISettingsProvider<TextReaderSettings> settingsProvider)
    {
        this.settingsProvider = settingsProvider;
    }

    public IReadOnlyList<string> SupportedFormats() => ["csv"];

    public Result<string> Read()
    {
        var settings = settingsProvider.GetSettings();
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };
        if (!File.Exists(settings.Path))
            return Result.Fail<string>($"File {settings.Path} does not exist.");
        try
        {
            using var reader = new StreamReader(settings.Path);
            using var csv = new CsvReader(reader, config);
            return string.Join(Environment.NewLine, csv.GetRecords<Cell>().Select(c => c.Word)).AsResult();
        }
        catch (Exception e)
        {
            return Result.Fail<string>(e.Message);
        }

    }

    private class Cell
    {
        [Index(0)]
        public string Word { get; set; }
    }
}
