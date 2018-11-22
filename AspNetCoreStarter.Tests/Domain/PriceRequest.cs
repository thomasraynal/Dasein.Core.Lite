using AspNetCoreStarterPack.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Tests.Domain
{
    public class PriceRequest : IHubRequest<IPrice>
    {
        public string GroupId => throw new NotImplementedException();

        public bool Accept(IPrice dto)
        {
            throw new NotImplementedException();
        }
    }
}
