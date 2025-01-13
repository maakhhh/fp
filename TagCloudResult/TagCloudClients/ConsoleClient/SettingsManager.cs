using TagCloud.BitmapGenerators;
using TagCloud.CloudImageSavers;
using TagCloud.CloudLayouter.PositionGenerator;
using TagCloud.TextReader;

namespace TagCloudClients.ConsoleClients;

public static class SettingsManager
{
    public static TextReaderSettings GetReaderSettings(Options options) =>
        options != null ? new(options.InputFilePath, options.Encoding) : new();

    public static BitmapGeneratorSettings GetBitmapSettings(Options options) =>
        options != null 
        ? new(options.ImageSize, options.BackgroundColor, options.WordColor, options.FontFamily)
        : new();

    public static SpiralGeneratorSettings GetSpiralSettings(Options options) =>
        options != null ? new(options.AngleOffset, options.SpiralStep, options.Center) : new();

    public static SaveSettings GetSaveSettings(Options options) =>
        options != null ? new(options.FileName, options.Format) : new();
}
