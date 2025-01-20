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
        Body body;
        try
        {
            using var doc = WordprocessingDocument.Open(settings.Path, false);
            if (doc.MainDocumentPart == null)
                return string.Empty.AsResult();
            body = doc.MainDocumentPart.Document.Body;
        }
        catch (Exception e)
        {
            return Result.Fail<string>(e.Message);
        }
        
        if (body == null) 
            return string.Empty.AsResult();
        var texts = body.Descendants<Text>().Select(x => x.Text);
        var sb = new StringBuilder();

        foreach(var text in texts.Where(t => t != string.Empty))
            sb.Append($"{text}{Environment.NewLine}");

        return sb.ToString().AsResult();
    }
}
