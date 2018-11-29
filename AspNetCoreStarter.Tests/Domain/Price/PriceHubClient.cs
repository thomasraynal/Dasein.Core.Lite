using AspNetCoreStarter.Tests.Module;
using AspNetCoreStarterPack;
using AspNetCoreStarterPack.Default;
using AspNetCoreStarterPack.Infrastructure;
using AspNetCoreStarterPack.SignalR;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Tests.Domain
{
    public class PriceHubClient : StreamingServiceClientBase<Price, PriceRequest>
    {
        private Func<HubConnectionBuilder> _builder;

        public PriceHubClient(PriceRequest request) : base(request)
        {
            _builder = () =>
            {
                return new HubConnectionBuilder().AddJsonProtocol(options =>
                                {
                                    options.PayloadSerializerSettings = AppCore.Instance.Get<JsonSerializerSettings>();
                                });
            };

        }

        public override string HubName => nameof(PriceHub);

        public override string OnStreamUpdateMethodName => TradeReferential.OnPriceChanged;

        public override Func<HubConnectionBuilder> ConnectionBuilderProvider => _builder;
    }
}
