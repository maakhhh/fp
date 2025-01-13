using System.Drawing;

namespace TagCloud.CloudLayouter;

public interface ICloudLayouter
{
    List<Rectangle> GetRectangles();
    Rectangle PutNextRectangle(Size rectangleSize);
}
