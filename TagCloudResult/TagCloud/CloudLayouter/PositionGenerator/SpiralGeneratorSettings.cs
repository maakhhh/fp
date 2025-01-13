using System.Drawing;

namespace TagCloud.CloudLayouter.PositionGenerator;

public class SpiralGeneratorSettings
{
    public double AngleOffset { get; private set; }
    public double SpiralStep { get; private set; }
    public Point Center { get; private set; }

    public SpiralGeneratorSettings(double angleOffset, double spiralStep, Point center)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(angleOffset);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(spiralStep);

        AngleOffset = angleOffset;
        SpiralStep = spiralStep;
        Center = center;
    }

    public SpiralGeneratorSettings() : this(0.5, 0.1, new(540, 960))
    {

    }
}
