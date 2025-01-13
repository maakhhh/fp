using System.Drawing.Imaging;

namespace TagCloud.CloudImageSavers;

public record SaveSettings
{
    public string FileName { get; private set; }
    public ImageFormat Format { get; private set; }

    public SaveSettings() : this("output", ImageFormat.Png)
    {
        
    }

    public SaveSettings(string fileName, ImageFormat format)
    {
        FileName = fileName;
        Format = format;
    }
}
