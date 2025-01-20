using CommandLine;
using ResultTools;
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

    public Result<None> Run()
        => Result.Of(() => Parser.Default.ParseArguments<Options>(args).Value)
            .Then(o =>
            {
                bitmapSettings.SetSettings(SettingsManager.GetBitmapSettings(o));
                saveSettings.SetSettings(SettingsManager.GetSaveSettings(o));
                spiralSettings.SetSettings(SettingsManager.GetSpiralSettings(o));
                readerSettings.SetSettings(SettingsManager.GetReaderSettings(o));
                
                return Result.Ok();
            })
            .Then(generator.GenerateCloud().AsResult())
            .Then(s => Console.WriteLine($"Result in path {s}"))
            .RefineError("Finished with error")
            .OnFail(Console.WriteLine);
}
