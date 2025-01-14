using System.Drawing;
using ResultTools;
using TagCloud.CloudLayouter.PositionGenerator;
using TagCloud.SettingsProviders;

namespace TagCloud.CloudLayouter;

public class SpiralPositionGenerator : IPositionGenerator
{
    private readonly ISettingsProvider<SpiralGeneratorSettings> settingsProvider;

    public SpiralPositionGenerator(ISettingsProvider<SpiralGeneratorSettings> settingsProvider)
    {
        this.settingsProvider = settingsProvider;
    }

    public IEnumerable<Result<Point>> GetPositions()
    {
        var settings = settingsProvider.GetSettings();
        if (settings.AngleOffset <= 0)
        {
            yield return Result.Fail<Point>($"{nameof(settings.AngleOffset)} must be greater than 0");
            yield break;
        }

        if (settings.SpiralStep <= 0)
        {
            yield return Result.Fail<Point>($"{nameof(settings.SpiralStep)} must be greater than 0");
            yield break;
        }
        
        int x, y;
        double radius, angle = 0;

        while (true)
        {
            radius = settings.SpiralStep * angle;
            x = (int)(settings.Center.X + radius * Math.Cos(angle));
            y = (int)(settings.Center.Y + radius * Math.Sin(angle));

            yield return new Point(x, y).AsResult();

            angle += settings.AngleOffset;
        }
    }
}
