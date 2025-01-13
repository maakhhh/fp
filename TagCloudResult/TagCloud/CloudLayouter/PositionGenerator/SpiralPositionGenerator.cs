using System.Drawing;
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

    public IEnumerable<Point> GetPositions()
    {
        var settings = settingsProvider.GetSettings();
        int x, y;
        double radius, angle = 0;

        while (true)
        {
            radius = settings.SpiralStep * angle;
            x = (int)(settings.Center.X + radius * Math.Cos(angle));
            y = (int)(settings.Center.Y + radius * Math.Sin(angle));

            yield return new(x, y);

            angle += settings.AngleOffset;
        }
    }
}
