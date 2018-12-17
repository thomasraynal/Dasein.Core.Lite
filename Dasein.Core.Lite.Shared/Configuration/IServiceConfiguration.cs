using Microsoft.Extensions.Configuration;

namespace Dasein.Core.Lite.Shared
{
    public interface IServiceConfiguration: IApplication
    {
        IAppContainer Container { get; }
        IConfiguration Root { get; }
        string Key { get; set; }
        long TokenExpiration { get; set; }
        long CacheDuration { get; set; }
        T GetServiceConfigurationValue<T>(string key);
    }
}