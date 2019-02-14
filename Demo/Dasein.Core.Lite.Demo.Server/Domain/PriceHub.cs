using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Dasein.Core.Lite.Demo.Server
{
    [Authorize]
    public class PriceHub : HubBase<Price>
    {
        public PriceHub(IHubContextHolder<Price> context) : base(context)
        {
        }

        public async Task RaisePriceChanged(Price price)
        {
            await RaiseChange(price, TradeServiceReferential.OnPriceChanged);
        }

        public override string Name => nameof(PriceHub);
    }
}
