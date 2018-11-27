using AspNetCoreStarter.Tests.Domain;
using AspNetCoreStarterPack;
using AspNetCoreStarterPack.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Tests.Module
{
    public class PriceHub : HubBase<Price>
    {
        public PriceHub(IHubContextHolder<Price> context) : base(context)
        {
        }

        public async Task RaisePriceChanged(Price price)
        {
            await RaiseChange(price, TradeReferential.OnPriceChanged);
        }

        public override string Name => nameof(PriceHub);
    }
}
