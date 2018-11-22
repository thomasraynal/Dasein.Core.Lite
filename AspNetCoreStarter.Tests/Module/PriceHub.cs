using AspNetCoreStarter.Tests.Domain;
using AspNetCoreStarterPack;
using AspNetCoreStarterPack.SignalR;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Tests.Module
{
    public class PriceHub : BaseHub<IPrice,PriceRequest>
    {
        public PriceHub(IHubContextHolder context) : base(context)
        {
        }

        public override string Name => nameof(PriceHub);
    }
}
