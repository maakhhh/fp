using ResultTools;
using TagCloud.SettingsProviders;

namespace TagCloud.TextReader;

public class TxtTextReader(ISettingsProvider<TextReaderSettings> settingsProvider) : ITextReader
{
    public IReadOnlyList<string> SupportedFormats() => ["txt"];

    public Result<string> Read()
    {
        var settings = settingsProvider.GetSettings();
        return File.Exists(settings.Path) 
            ? File.ReadAllText(settings.Path).AsResult() 
            : Result.Fail<string>($"File {settings.Path} does not exist.");
    }
}
