using System.Drawing;
using TagCloud.CloudLayouter.PositionGenerator;
using TagCloud.SettingsProviders;

namespace TagCloud.CloudLayouter;

public static class CloudGenerator
{
    public static ICloudLayouter GenerateRandomCloudWithCenter(Point center, int rectangleCount)
    {
        var settingsProvider = new SettingsProvider<SpiralGeneratorSettings>();
        settingsProvider.SetSettings(new(0.5, 0.1, center));
        var random = new Random();
        var layouter = new CircularCloudLayouter(
            new SpiralPositionGenerator(settingsProvider));

        for (var i = 0; i < rectangleCount; i++)
        {
            var width = random.NextInt64(10, 70);
            var height = random.NextInt64(10, 70);
            layouter.PutNextRectangle(new((int)width, (int)height));
        }

        return layouter;
    }
}
