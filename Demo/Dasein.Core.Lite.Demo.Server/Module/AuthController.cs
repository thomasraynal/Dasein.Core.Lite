using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Dasein.Core.Lite.Shared;

namespace Dasein.Core.Lite.Demo.Server
{
    public class AuthController : ServiceControllerBase
    {
        private IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<TradeServiceToken> Login([FromBody] Credentials credentials)
        {
            return await _userService.Login(credentials);
        }

    }
}
