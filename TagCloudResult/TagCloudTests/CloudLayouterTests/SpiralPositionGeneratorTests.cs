using FluentAssertions;
using System.Drawing;
using TagCloud.CloudLayouter;
using TagCloud.CloudLayouter.PositionGenerator;
using TagCloud.SettingsProviders;


namespace TagsCloudVisualizationTests;

[TestFixture]
public class SpiralPositionGeneratorTests
{
    private SettingsProvider<SpiralGeneratorSettings> settingsProvider = new();

    [TestCase(0, TestName = "Zero step")]
    [TestCase(-1, TestName = "Negative step")]
    public void GeneratorConstructor_ThrowArgumentException_WithUncorrectStep(double step)
    {
        Action action = () => settingsProvider.SetSettings(new(0.5, step, Point.Empty));
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestCase(0, 0, TestName = "Zero center")]
    [TestCase(1, 1, TestName = "Not zero center")]
    public void Generator_ReturnFirstPoint_LikeCenter(int centerX, int centerY)
    {
        var center = new Point(centerX, centerY);
        settingsProvider.SetSettings(new(0.5, 0.1, center));
        var generator = new SpiralPositionGenerator(settingsProvider);
        var firstPoint = generator.GetPositions().First();

        firstPoint.Should().Be(center);
    }

    [Test]
    public void Generator_ReturnPoints_MovingAwayFromCenter()
    {
        var center = new Point(0, 0);
        settingsProvider.SetSettings(new(0.5, 1, center));
        var points = new SpiralPositionGenerator(settingsProvider)
            .GetPositions().Take(1000).Where((_, id) => id % 10 == 0);
        var prevDistance = double.MinValue;

        foreach (var point in points)
        {
            var distance = GetDistanceBetweenPoints(center, point);
            distance.Should().BeGreaterThan(prevDistance);
            prevDistance = distance;
        }
    }

    private double GetDistanceBetweenPoints(Point point1, Point point2)
    => Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
}
