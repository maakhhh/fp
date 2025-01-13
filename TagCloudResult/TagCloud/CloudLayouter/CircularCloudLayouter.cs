using System.Drawing;

namespace TagCloud.CloudLayouter;
public class CircularCloudLayouter : ICloudLayouter
{
    private readonly List<Rectangle> rectangles;
    private readonly IEnumerator<Point> pointEnumerator;

    public CircularCloudLayouter(IPositionGenerator generator)
    {
        rectangles = new();
        pointEnumerator = generator.GetPositions().GetEnumerator();
    }

    public List<Rectangle> GetRectangles() => rectangles;

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException(
                $"{nameof(rectangleSize)} должен иметь высоту и ширину больше нуля, передано ({rectangleSize.Width} {rectangleSize.Height})"
            );

        Rectangle rectangle;

        do
            rectangle = PutRectangleInNextPosition(rectangleSize);
        while (rectangles.Any(r => r.IntersectsWith(rectangle)));

        rectangles.Add(rectangle);
        return rectangle;
    }

    private Rectangle PutRectangleInNextPosition(Size rectagleSize)
    {
        pointEnumerator.MoveNext();
        var centerOfRectangle = pointEnumerator.Current;
        var rectanglePosition = GetPositionFromCenter(centerOfRectangle, rectagleSize);
        return new(rectanglePosition, rectagleSize);
    }

    private Point GetPositionFromCenter(Point center, Size size) => 
        new(center.X - size.Width / 2, center.Y - size.Height / 2);
}
