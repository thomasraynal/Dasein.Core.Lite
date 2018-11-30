using Dasein.Core.Lite.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Service.Demo.Desktop
{
    public class HubConfiguration : IHubConfiguration
    {
        private HubDescriptor[] _hubs;

        public HubConfiguration()
        {
            _hubs = new HubDescriptor[]
            {
                new HubDescriptor()
                {
                    Endpoints= new string[]{ "http://localhost:8080/hub/price" },
                    Name = "PriceHub"
                },
                                new HubDescriptor()
                {
                    Endpoints= new string[]{ "http://localhost:8080/hub/trade" },
                    Name = "TradeEventHub"
                }
            };
        }

        public HubDescriptor[] Hubs => _hubs;
    }
}
