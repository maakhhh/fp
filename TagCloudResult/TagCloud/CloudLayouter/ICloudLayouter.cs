using System.Drawing;
using ResultTools;

namespace TagCloud.CloudLayouter;

public interface ICloudLayouter
{
    List<Rectangle> GetRectangles();
    Result<Rectangle> PutNextRectangle(Size rectangleSize);
}
