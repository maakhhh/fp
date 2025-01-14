using System.Drawing;
using ResultTools;

namespace TagCloud.CloudLayouter;

public interface IPositionGenerator
{
    IEnumerable<Result<Point>> GetPositions();
}
