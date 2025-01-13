using System.Drawing;

namespace TagCloud.BitmapGenerators;

public record BitmapGeneratorSettings
{
    public Size ImageSize { get; private set; }
    public Color BackgroundColor { get; private set; }
    public Color WordColor { get; private set; }
    public FontFamily FontFamily { get; private set; }

    public BitmapGeneratorSettings()
    : this(new Size(1080, 1920), Color.White, Color.Black, new FontFamily("Arial"))
    {
        
    }

    public BitmapGeneratorSettings(
        Size imageSize,
        Color backgroudColor,
        Color wordColor,
        FontFamily fontFamily)
    {
        ImageSize = imageSize;
        BackgroundColor = backgroudColor;
        WordColor = wordColor;
        FontFamily = fontFamily;
    }
}
