using Autofac;
using TagCloud;
using TagCloud.BitmapGenerators;
using TagCloud.CloudImageSavers;
using TagCloud.CloudLayouter;
using TagCloud.CloudLayouter.PositionGenerator;
using TagCloud.SettingsProviders;
using TagCloud.TextFilters;
using TagCloud.TextReader;
using TagCloud.TextSplitters;
using TagCloudClients.ConsoleClients;

namespace TagCloudClients;

internal static class ContainerBuilderExtensions
{
    public static ContainerBuilder WithServices(this ContainerBuilder builder)
    {
        builder.RegisterType<BitmapGenerator>().As<IBitmapGenerator>()
            .SingleInstance();
        builder.RegisterType<CloudImageSaver>().As<ICloudImageSaver>()
            .SingleInstance();
        builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>()
            .SingleInstance();
        builder.RegisterType<SpiralPositionGenerator>().As<IPositionGenerator>()
            .SingleInstance();
        builder.RegisterType<BoringTextFilter>().As<ITextFilter>()
            .SingleInstance();
        builder.RegisterType<LowercaseTextFilter>().As<ITextFilter>()
            .SingleInstance();
        builder.RegisterType<TxtTextReader>().As<ITextReader>()
            .SingleInstance();
        builder.RegisterType<DocxTextReader>().As<ITextReader>()
            .SingleInstance();
        builder.RegisterType<CsvTextReader>().As<ITextReader>()
            .SingleInstance();
        builder.RegisterType<NewLineTextSplitter>().As<ITextSplitter>()
            .SingleInstance();
        builder.RegisterType<TextReaderProvider>().AsSelf()
            .SingleInstance();
        builder.RegisterType<TagCloudImageGenerator>().AsSelf()
            .SingleInstance();

        return builder;
    }

    public static ContainerBuilder WithSettings(this ContainerBuilder builder)
    {
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

        return builder;
    }

    public static ContainerBuilder WithConsoleClient(this ContainerBuilder builder, string[] args)
    {
        builder.RegisterInstance(args).SingleInstance();;
        builder.RegisterType<ConsoleClient>().As<IClient>().SingleInstance();;
        return builder;
    }
}
