using System.Drawing;
using TagCloud.CloudLayouter;
using TagCloud.SettingsProviders;

namespace TagCloud.BitmapGenerators;

public class BitmapGenerator: IBitmapGenerator
{
    private readonly ISettingsProvider<BitmapGeneratorSettings> settingsProvider;
    private readonly ICloudLayouter layouter;

    public BitmapGenerator(ICloudLayouter layouter, ISettingsProvider<BitmapGeneratorSettings> settingsProvider)
    {
        this.settingsProvider = settingsProvider;
        this.layouter = layouter;
    }
    
    public Bitmap GenerateBitmapFromWords(IEnumerable<CloudWord> words)
    {
        var padding = 1.5f;
        var settings = settingsProvider.GetSettings();
        var bitmap = new Bitmap(settings.ImageSize.Width, settings.ImageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);

        graphics.Clear(settings.BackgroundColor);
        using var brush = new SolidBrush(settings.WordColor);

        foreach (var word in words)
        {
            using var font = new Font(settings.FontFamily, word.FontSize);
            var size = graphics.MeasureString(word.Word, font);
            var position = layouter.PutNextRectangle(size.ToSize());
            var textPosition = new PointF(position.X + padding, position.Y + padding);
            graphics.DrawString(word.Word, font, brush, textPosition);
        }

        return bitmap;
    }
}
