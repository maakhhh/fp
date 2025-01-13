using TagCloud.SettingsProviders;

namespace TagCloud.TextReader;

public class TxtTextReader(ISettingsProvider<TextReaderSettings> settingsProvider) : ITextReader
{
    public IReadOnlyList<string> SupportedFormats() => ["txt"];

    public string Read()
    {
        var settings = settingsProvider.GetSettings();
        return File.ReadAllText(settings.Path, settings.Encoding);
    }
}
