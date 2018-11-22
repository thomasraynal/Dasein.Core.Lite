using Microsoft.Extensions.Configuration;

namespace AspNetCoreStarterPack
{
    public interface IServiceConfiguration
    {
        IAppContainer Container { get; }
        IConfiguration Root { get; }
        string Name { get; set; }
        int Version { get; set; }
    }
}