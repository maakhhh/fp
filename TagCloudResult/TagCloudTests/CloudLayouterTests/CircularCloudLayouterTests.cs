using FluentAssertions;
using NUnit.Framework.Interfaces;
using System.Drawing;
using TagCloud.CloudLayouter;
using TagCloud.CloudLayouter.PositionGenerator;
using TagCloud.SettingsProviders;


namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private SettingsProvider<SpiralGeneratorSettings> settingsProvider;

    [SetUp]
    public void SetUp()
    {
        settingsProvider = new SettingsProvider<SpiralGeneratorSettings>();
    }

    [TestCase(0, 1, TestName = "Zero width")]
    [TestCase(1, 0, TestName = "Zero height")]
    [TestCase(-1, 1, TestName = "Negative width")]
    [TestCase(1, -1, TestName = "Negative height")]
    public void Layouter_ReturnError_WithUncorrectData(int width, int height)
    {
        settingsProvider.SetSettings(new(0.1, 0.5, Point.Empty));
        var layouter = new CircularCloudLayouter(
            new SpiralPositionGenerator(settingsProvider));
        var size = new Size(width, height);
        var result = layouter.PutNextRectangle(size);
        
        result.IsSuccess.Should().BeFalse();
    }

    [TestCase(0, 0, TestName = "Zero center")]
    [TestCase(1, 1, TestName = "Non-zero center")]
    public void LayouterPutFirstRectangleInCenter(int x, int y)
    {
        settingsProvider.SetSettings(new(0.5, 0.1, new(x, y)));
        var layouter = new CircularCloudLayouter(
            new SpiralPositionGenerator(settingsProvider));
        var rectangleSize = new Size(10, 10);

        var actualRectangle = layouter.PutNextRectangle(rectangleSize);
        var expectedRectangle = new Rectangle(
            -rectangleSize.Width / 2 + x,
            -rectangleSize.Height / 2 + y,
            rectangleSize.Width,
            rectangleSize.Height
        );
        
        actualRectangle.IsSuccess.Should().BeTrue();
        actualRectangle.Value.Should().BeEquivalentTo(expectedRectangle);
    }

    [Test]
    [Repeat(5)]
    public void RectanglesHaveNotIntersects()
    {
        var random = new Random();
        var count = random.Next(10, 30);

        var layouter = CloudGenerator.GenerateRandomCloudWithCenter(new(0, 0), count);

        AreRectanglesHaveIntersects(layouter.GetRectangles()).Should().BeFalse();
    }

    [Test]
    [Repeat(5)]
    public void RectanglesCenterShoulBeLikeInitCenter()
    {
        var center = new Point(0, 0);
        var treshold = 25;
        var layouter = CloudGenerator.GenerateRandomCloudWithCenter(center, 100);

        var actualCenter = GetCenterOfRectangles(layouter.GetRectangles());

        actualCenter.X.Should().BeInRange(center.X - treshold, center.X + treshold);
        actualCenter.Y.Should().BeInRange(center.Y - treshold, center.Y + treshold);
    }

    [Test]
    [Repeat(5)]
    public void RectanglesDensityShouldBeMaximum()
    {
        var expectedDensity = 0.45;
        var center = new Point(0, 0);
        var layouter = CloudGenerator.GenerateRandomCloudWithCenter(center, 100);

        var rectanglesArea = layouter.GetRectangles().Sum(r => r.Width * r.Height);
        var radius = GeMaxDistanceFromRectangleToCenter(layouter.GetRectangles());
        var circleArea = Math.PI * radius * radius;
        var density = rectanglesArea / circleArea;

        density.Should().BeGreaterThanOrEqualTo(expectedDensity);

    }

    private double GeMaxDistanceFromRectangleToCenter(List<Rectangle> rectangles)
    {
        var center = GetCenterOfRectangles(rectangles);

        double maxDistance = 0;

        foreach (var rectangle in rectangles)
        {
            var corners = new Point[4]
            {
                new(rectangle.Top, rectangle.Left),
                new(rectangle.Top, rectangle.Right),
                new(rectangle.Bottom, rectangle.Left),
                new(rectangle.Bottom, rectangle.Right)
            };

            var distance = corners.Max(p => GetDistanceBetweenPoints(p, center));
            maxDistance = Math.Max(maxDistance, distance);
        }

        return maxDistance;
    }

    private Point GetCenterOfRectangles(List<Rectangle> rectangles)
    {
        var top = rectangles.Max(r => r.Top);
        var left = rectangles.Min(r => r.Left);
        var bottom = rectangles.Min(r => r.Bottom);
        var right = rectangles.Max(r => r.Right);

        var x = left + (right - left) / 2;
        var y = bottom + (top - bottom) / 2;

        return new(x, y);
    }

    private bool AreRectanglesHaveIntersects(List<Rectangle> rectangles)
    {
        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                if (rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
            }
        }

        return false;
    }

    private double GetDistanceBetweenPoints(Point point1, Point point2)
        => Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
}