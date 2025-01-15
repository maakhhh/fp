using System.Drawing;
using ResultTools;

namespace TagCloud.CloudLayouter.PositionGenerator;

public interface IPositionGenerator
{
    IEnumerable<Result<Point>> GetPositions();
}
