using Dasein.Core.Lite.Shared;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace Dasein.Core.Lite.Demo.Shared
{
    public class PriceHubClient : SignalRServiceClientBase<Price, PriceRequest>
    {
        private Func<HubConnectionBuilder> _builder;

        public PriceHubClient(PriceRequest request, HttpTransportType transports, Action<HttpConnectionOptions> configureHttpConnection) : base(request, transports, configureHttpConnection)
        {
        }

        protected override void Initialize()
        {
            _builder = () =>
            {
                return new HubConnectionBuilder().AddJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings = AppCore.Instance.Get<JsonSerializerSettings>();
                });
            };
        }

        public override string HubName => TradeServiceReferential.PriceHub;

        public override string OnStreamUpdateMethodName => TradeServiceReferential.OnPriceChanged;

        public override Func<HubConnectionBuilder> ConnectionBuilderProvider => _builder;
    }
}
