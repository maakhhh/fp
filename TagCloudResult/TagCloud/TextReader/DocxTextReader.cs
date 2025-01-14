using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;
using ResultTools;
using TagCloud.SettingsProviders;

namespace TagCloud.TextReader;

public class DocxTextReader : ITextReader
{
    private readonly ISettingsProvider<TextReaderSettings> settingsProvider;

    public DocxTextReader(ISettingsProvider<TextReaderSettings> settingsProvider)
    {
        this.settingsProvider = settingsProvider;
    }

    public IReadOnlyList<string> SupportedFormats() => ["doc", "docx"];

    public Result<string> Read()
    {
        var settings = settingsProvider.GetSettings();
        if (!File.Exists(settings.Path))
            return Result.Fail<string>($"File {settings.Path} does not exist.");
        using var doc = WordprocessingDocument.Open(settings.Path, false);
        if (doc == null || doc.MainDocumentPart == null)
            return string.Empty.AsResult();
        var body = doc.MainDocumentPart.Document.Body;

        if (body == null) 
            return string.Empty.AsResult();
        var texts = body.Descendants<Text>().Select(x => x.Text);
        var sb = new StringBuilder();

        foreach(var text in texts.Where(t => t != string.Empty))
            sb.Append($"{text}{Environment.NewLine}");

        return sb.ToString().AsResult();
    }
}
