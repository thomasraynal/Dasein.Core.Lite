﻿using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite;
using Dasein.Core.Lite.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Dasein.Core.Lite.Demo.Server
{
    public class TradeServiceProxy : ServiceProxyBase<ITradeService>, ITradeService
    {
        private ISignalRService<TradeEvent, TradeEventRequest> _tradeEventService;

        public static string middlewareKey = "PROCESSED_BY_MIDDLEWARE";

        public TradeServiceProxy(IUserService userService)
        {
            Task.Delay(500).ContinueWith((_) =>
            {
                var token = userService.Login(new Credentials()
                {
                    Username = "APP SERVICE",
                    Password = "idkfa"

                }).Result;

                _tradeEventService = SignalRServiceBuilder<TradeEvent, TradeEventRequest>
                                  .Create()
                                  .Build(new TradeEventRequest((p) => true), (opts) =>
                                  {
                                      opts.AccessTokenProvider = () => Task.FromResult(token.Digest);
                                  });

                _tradeEventService.Connect(Scheduler.Default, 0);
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

                //if signalr is still trying to get a running instance, just bypass the process... 
                if (null == _tradeEventService.Current) return;

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

        public async Task<IEnumerable<ITrade>> GetAllTradesViaMiddleware()
        {
            var trades = await Service.GetAllTrades();
            return trades.Select(trade => new Trade(trade.Id, trade.Date, trade.Counterparty, $"{trade.Asset} [{middlewareKey}]", trade.Status, trade.Way, trade.PriceOnTransaction, trade.Volume));
        }

        public async Task<ITrade> GetTradeById(Guid tradeId)
        {
            return await Service.GetTradeById(tradeId);
        }

        public Task<IEnumerable<ITrade>> GetAllTradesViaCache()
        {
            throw new NotImplementedException();
        }
    }
}
