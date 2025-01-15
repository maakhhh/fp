using System.Drawing;
using ResultTools;
using TagCloud.CloudLayouter.PositionGenerator;

namespace TagCloud.CloudLayouter;
public class CircularCloudLayouter : ICloudLayouter
{
    private readonly List<Rectangle> rectangles;
    private readonly IEnumerator<Result<Point>> pointEnumerator;

    public CircularCloudLayouter(IPositionGenerator generator)
    {
        rectangles = new();
        pointEnumerator = generator.GetPositions().GetEnumerator();
    }

    public List<Rectangle> GetRectangles() => rectangles;

    public Result<Rectangle> PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            return Result.Fail<Rectangle>(
                $"{nameof(rectangleSize)} должен иметь высоту и ширину больше нуля," +
                $" передано ({rectangleSize.Width} {rectangleSize.Height})");

        Result<Rectangle> rectangle;

        do
        {
            rectangle = PutRectangleInNextPosition(rectangleSize);
            if (!rectangle.IsSuccess)
                return rectangle;
        }
        while (rectangles.Any(r => r.IntersectsWith(rectangle.Value)));

        rectangles.Add(rectangle.Value);
        return rectangle;
    }

    private Result<Rectangle> PutRectangleInNextPosition(Size rectagleSize)
    {
        pointEnumerator.MoveNext();
        var centerOfRectangle = pointEnumerator.Current;
        return centerOfRectangle
            .Then(c => new Rectangle(
                GetPositionFromCenter(c, rectagleSize),
                rectagleSize));
    }

    private Point GetPositionFromCenter(Point center, Size size) => 
        new(center.X - size.Width / 2, center.Y - size.Height / 2);
}
