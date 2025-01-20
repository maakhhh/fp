using ResultTools;

namespace TagCloudClients;

public interface IClient
{
    Result<None> Run();
}
