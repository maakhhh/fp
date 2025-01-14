using System.Drawing;
using ResultTools;
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
    
    public Result<Bitmap> GenerateBitmapFromWords(IEnumerable<CloudWord> words)
    {
        var padding = 1.5f;
        var settings = settingsProvider.GetSettings();
        
        var bitmap = settings.ImageSize is {Width: > 0, Height: > 0}
            ? new Bitmap(settings.ImageSize.Width, settings.ImageSize.Height).AsResult()

            : Result.Fail<Bitmap>("Bitmap size must be grater than zero");

        return bitmap
            .Then(b =>
            {
                using var graphics = Graphics.FromImage(b);

                graphics.Clear(settings.BackgroundColor);
                
                var result = DrawWords(words, graphics, settings);
                return result.IsSuccess
                    ? bitmap
                    : Result.Fail<Bitmap>(result.Error!);
            });
    }
    
    private Result<None> DrawWords(IEnumerable<CloudWord> words, Graphics graphics, BitmapGeneratorSettings settings)
    {
        var padding = 1.5f;
        using var brush = new SolidBrush(settings.WordColor);
        foreach (var word in words)
        {
            using var font = new Font(settings.FontFamily, word.FontSize);
            var size = graphics.MeasureString(word.Word, font);
            var position = layouter.PutNextRectangle(size.ToSize());
            return position
                .Then(p => new Rectangle(Point.Empty, settings.ImageSize).Contains(p)
                    ? p.AsResult()
                    : Result.Fail<Rectangle>("Point is out of bounds"))
                .Then(p => new PointF(p.X + padding, p.Y + padding))
                .Then(p =>
                {
                    graphics.DrawString(word.Word, font, brush, p);
                    return Result.Ok();
                });
        }
        
        return Result.Ok();
    }
    
    
}

