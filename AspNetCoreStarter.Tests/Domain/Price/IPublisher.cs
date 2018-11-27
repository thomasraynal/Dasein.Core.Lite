using System.Threading.Tasks;

namespace AspNetCoreStarter.Demo.Common.Domain
{
    public interface IPublisher
    {
        Task Start();
    }
}