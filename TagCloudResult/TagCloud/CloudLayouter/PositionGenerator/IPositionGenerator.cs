using System.Drawing;

namespace TagCloud.CloudLayouter;

public interface IPositionGenerator
{
    IEnumerable<Point> GetPositions();
}
