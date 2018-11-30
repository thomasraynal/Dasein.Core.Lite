using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Demo.Server
{
    public class TradeEventHub : HubBase<TradeEvent>
    {
        public TradeEventHub(IHubContextHolder<TradeEvent> context) : base(context)
        {
        }

        public override string Name => nameof(TradeEventHub);

        public async Task RaiseTradeEvent(TradeEvent @event)
        {
            await RaiseChange(@event, TradeServiceReferential.OnTradeEvent);
        }
    }
}
