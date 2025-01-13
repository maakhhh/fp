using ApprovalTests;
using ApprovalTests.Reporters;
using Autofac;
using FluentAssertions;
using TagCloud;
using TagCloud.BitmapGenerators;
using TagCloud.CloudImageSavers;
using TagCloud.CloudLayouter;
using TagCloud.CloudLayouter.PositionGenerator;
using TagCloud.SettingsProviders;
using TagCloud.TextFilters;
using TagCloud.TextReader;
using TagCloud.TextSplitters;
using TagCloudClients;
using TagCloudClients.ConsoleClients;

namespace TagCloudTests;

[TestFixture]
[UseReporter(typeof(VisualStudioReporter))]
[ApprovalTests.Namers.UseApprovalSubdirectory("Samples")]
public class TagCloudTests
{
    [Test]
    public void ApprovalTest()
    {
        var client = InitCLient();
        client.Run();

        File.Exists("output.png").Should().BeTrue();
        Approvals.VerifyFile("output.png");
    }

    private IClient InitCLient()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<BitmapGenerator>().As<IBitmapGenerator>();
        builder.RegisterType<CloudImageSaver>().As<ICloudImageSaver>();
        builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
        builder.RegisterType<SpiralPositionGenerator>().As<IPositionGenerator>();
        builder.RegisterType<BoringTextFilter>().As<ITextFilter>();
        builder.RegisterType<LowercaseTextFilter>().As<ITextFilter>();
        builder.RegisterType<TxtTextReader>().As<ITextReader>();
        builder.RegisterType<DocxTextReader>().As<ITextReader>();
        builder.RegisterType<CsvTextReader>().As<ITextReader>();
        builder.RegisterType<NewLineTextSplitter>().As<ITextSplitter>();
        builder.RegisterType<TextReaderProvider>().AsSelf();
        builder.RegisterType<TagCloudImageGenerator>().AsSelf();
        builder.RegisterType<SettingsProvider<BitmapGeneratorSettings>>()
            .As<ISettingsProvider<BitmapGeneratorSettings>, ISettingsHolder<BitmapGeneratorSettings>>()
            .SingleInstance();
        builder.RegisterType<SettingsProvider<SaveSettings>>()
            .As<ISettingsProvider<SaveSettings>, ISettingsHolder<SaveSettings>>()
            .SingleInstance();
        builder.RegisterType<SettingsProvider<SpiralGeneratorSettings>>()
            .As<ISettingsProvider<SpiralGeneratorSettings>, ISettingsHolder<SpiralGeneratorSettings>>()
            .SingleInstance();
        builder.RegisterType<SettingsProvider<TextReaderSettings>>()
            .As<ISettingsProvider<TextReaderSettings>, ISettingsHolder<TextReaderSettings>>()
            .SingleInstance();

        builder.RegisterInstance(new string[2] { "-i", "Samples/input.csv" });
        builder.RegisterType<ConsoleClient>().As<IClient>();
        using var container = builder.Build();
        return container.Resolve<IClient>();
    }
}
