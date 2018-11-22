using AspNetCoreStarter.Authentication;
using AspNetCoreStarter.Tests.Domain;
using AspNetCoreStarterPack;
using AspNetCoreStarterPack.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
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
        public IActionResult Login([FromBody] CredentialsDto credentials)
        {
            return Ok(_userService.Login(credentials));
        }

    }
}
