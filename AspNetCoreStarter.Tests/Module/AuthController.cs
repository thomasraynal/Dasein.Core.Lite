using AspNetCoreStarter.Tests.Domain;
using AspNetCoreStarter.Tests.Infrastructure;
using AspNetCoreStarterPack;
using AspNetCoreStarterPack.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Tests.Module
{
    public class AuthController : ServiceControllerBase
    {
        private IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<TradeServiceToken> Login([FromBody] CredentialsDto credentials)
        {
            return await _userService.Login(credentials);
        }

    }
}
