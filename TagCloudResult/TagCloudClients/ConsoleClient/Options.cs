using CommandLine;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace TagCloudClients.ConsoleClients;

public class Options
{
    [Option('i', "inputFilePath", HelpText = "Path to input file with words")]
    public string InputFilePath { get; set; } = "input.txt";

    [Option('e', "encodingForInputFile", HelpText = "Encoding for input file")]
    public Encoding Encoding { get; set; } = Encoding.UTF8;

    [Option('s', "imageSize", HelpText = "Size of result image")]
    public Size ImageSize { get; set; } = new(1080, 1920);

    [Option('b', "backgroundColor", HelpText = "Color of result image background")]
    public Color BackgroundColor { get; set; } = Color.White;

    [Option('w', "wordColor", HelpText = "Color of words in result image")]
    public Color WordColor { get; set; } = Color.Black;

    [Option('f', "fontFamily", HelpText = "Font family of words in result image")]
    public FontFamily FontFamily { get; set; } = new FontFamily("Times New Roman");

    [Option('a', "angleOffset", HelpText = "Angle offset for spiral in result cloud")]
    public double AngleOffset { get; set; } = 0.1;

    [Option("spiralStep", HelpText = "Step of spiral for cloud")]
    public double SpiralStep { get; set; } = 0.5;

    [Option('c', "centerOfCloud", HelpText = "Center of words cloud")]
    public Point Center { get; set; } = new(540, 960);

    [Option('o', "outputFileName", HelpText = "FileName for result image")]
    public string FileName { get; set; } = "output";

    [Option("imageFormat", HelpText = "Format (png, jpg, jpeg) of result image")]
    public ImageFormat Format { get; set; } = ImageFormat.Png;
}
