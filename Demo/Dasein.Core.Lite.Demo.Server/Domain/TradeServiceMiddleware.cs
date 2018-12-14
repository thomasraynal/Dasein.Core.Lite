﻿using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite;
using Dasein.Core.Lite.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Demo.Server
{
    public class TradeServiceMiddleware : MiddlewareBase<ITradeService>, ITradeService
    {
        private ISignalRService<TradeEvent, TradeEventRequest> _tradeEventService;

        public TradeServiceMiddleware()
        {
            Task.Delay(1000).ContinueWith((_) =>
            {
                _tradeEventService = SignalRServiceBuilder<TradeEvent, TradeEventRequest>
                                  .Create()
                                  .Build(new TradeEventRequest((p) => true));

                _tradeEventService.Connect(Scheduler.Default, 1000);
            });

        }

        public async Task<TradeCreationResult> CreateTrade(TradeCreationRequest request)
        {
            var trade = await Service.CreateTrade(request);

            //run "compliance" on a "compliance service"
            Scheduler.Default.Schedule(async () =>
            {
                //wait for the client to fetch the new trade...
                await Task.Delay(1000);

                await _tradeEventService.Current.Proxy.InvokeAsync(TradeServiceReferential.RaiseTradeEvent, new TradeEvent()
                {
                    Status = TradeStatus.ComplianceCheck,
                    TradeId = trade.TradeId
                });

                if (request.Counterparty == TradeServiceReferential.HighLatencyCounterparty)
                {
                    await Task.Delay(5000);
                }
                else
                {
                    await Task.Delay(1000);
                }

                var status = TradeStatus.Processed;

                //reject 1/5 trades
                if (new Random().Next(1, 6) == 1)
                {
                    status = TradeStatus.Rejected;
                }

                await _tradeEventService.Current.Proxy.InvokeAsync(TradeServiceReferential.RaiseTradeEvent, new TradeEvent()
                {
                    Status = status,
                    TradeId = trade.TradeId
                });

            });

            return trade;
        }

        public async Task<IEnumerable<ITrade>> GetAllTrades()
        {
            return await Service.GetAllTrades();
        }

        public async Task<ITrade> GetTradeById(Guid tradeId)
        {
            return await Service.GetTradeById(tradeId);
        }
    }
}
