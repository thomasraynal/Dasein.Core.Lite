using AspNetCoreStarter.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Tests.Infrastructure
{
    public class TradeServiceToken : ServiceToken
    {
        public string Username { get; set; }
    }
}
