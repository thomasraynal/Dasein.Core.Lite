using AspNetCoreStarter.Authentication;
using AspNetCoreStarter.Tests.Infrastructure;
using AspNetCoreStarterPack.Authentication;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Tests.Domain
{
    public interface IUserService
    {
        Task<TradeServiceToken> Login(CredentialsDto credentials);
    }
}