﻿using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite.Shared;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Demo.Server
{
    public class TradeEventHubClient : SignalRServiceClientBase<TradeEvent, TradeEventRequest>
    {
        private Func<HubConnectionBuilder> _builder;

        public TradeEventHubClient(TradeEventRequest request, HttpTransportType transports, Action<HttpConnectionOptions> configureHttpConnection) : base(request, transports, configureHttpConnection)
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

        public override string HubName => TradeServiceReferential.TradeEventHub;

        public override Func<HubConnectionBuilder> ConnectionBuilderProvider => _builder;
    }
}
