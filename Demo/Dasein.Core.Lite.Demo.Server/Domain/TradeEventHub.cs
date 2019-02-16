using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Dasein.Core.Lite.Demo.Server
{
    [Authorize]
    public class TradeEventHub : HubBase<TradeEvent>
    {
        public TradeEventHub(IHubContextHolder<TradeEvent> context) : base(context)
        {
        }

        public override string Name => nameof(TradeEventHub);
    }
}
