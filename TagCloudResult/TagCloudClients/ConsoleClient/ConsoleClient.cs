using CommandLine;
using TagCloud;
using TagCloud.BitmapGenerators;
using TagCloud.CloudImageSavers;
using TagCloud.CloudLayouter.PositionGenerator;
using TagCloud.SettingsProviders;
using TagCloud.TextReader;

namespace TagCloudClients.ConsoleClients;

public class ConsoleClient : IClient
{
    private readonly TagCloudImageGenerator generator;
    private readonly ISettingsHolder<BitmapGeneratorSettings> bitmapSettings;
    private readonly ISettingsHolder<SpiralGeneratorSettings> spiralSettings;
    private readonly ISettingsHolder<SaveSettings> saveSettings;
    private readonly ISettingsHolder<TextReaderSettings> readerSettings;
    private readonly string[] args;

    public ConsoleClient(
        TagCloudImageGenerator generator,
        ISettingsHolder<BitmapGeneratorSettings> bitmapSettings,
        ISettingsHolder<SaveSettings> saveSettings,
        ISettingsHolder<SpiralGeneratorSettings> spiralSettings,
        ISettingsHolder<TextReaderSettings> readerSettings,
        string[] args)
    {
        this.generator = generator;
        this.bitmapSettings = bitmapSettings;
        this.saveSettings = saveSettings;
        this.readerSettings = readerSettings;
        this.spiralSettings = spiralSettings;
        this.args = args;
    }

    public void Run()
    {
        var options = Parser.Default.ParseArguments<Options>(args).Value;

        bitmapSettings.SetSettings(SettingsManager.GetBitmapSettings(options));
        saveSettings.SetSettings(SettingsManager.GetSaveSettings(options));
        spiralSettings.SetSettings(SettingsManager.GetSpiralSettings(options));
        readerSettings.SetSettings(SettingsManager.GetReaderSettings(options));

        var output = generator.GenerateCloud();
        Console.WriteLine($"Cloud image save to path {output}");
    }
}
