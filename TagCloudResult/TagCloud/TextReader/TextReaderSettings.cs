using System.Text;

namespace TagCloud.TextReader;

public record class TextReaderSettings
{
    public string Path { get; private set; }
    public Encoding Encoding { get; private set; }

    public TextReaderSettings() : this("input.txt", Encoding.UTF8)
    {
        
    }

    public TextReaderSettings(string path, Encoding encoding)
    {
        Path = path;
        Encoding = encoding;
    }
}
