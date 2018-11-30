using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Demo.Shared
{
    public class TradeEvent
    {
        public Guid TradeId { get; set; }

        public TradeStatus Status { get; set; }
    }
}
