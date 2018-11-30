using Microsoft.Extensions.Configuration;

namespace Dasein.Core.Lite.Shared
{
    public interface IServiceConfiguration: IApplication
    {
        IAppContainer Container { get; }
        IConfiguration Root { get; }
    }
}