using Autofac;

namespace TagCloudClients;

public static class Program
{
    public static void Main(string[] args)
    {
        using var container = new ContainerBuilder()
            .WithServices()
            .WithSettings()
            .WithConsoleClient(args)
            .Build();

        container.Resolve<IClient>().Run();
    }
}