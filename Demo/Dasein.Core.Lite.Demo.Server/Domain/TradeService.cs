﻿using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite;
using Dasein.Core.Lite.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Demo.Server
{
    public class TradeService : IService<ITradeService>, ITradeService, ICanLog
    {
        private readonly List<ITrade> _repository;
        private ISignalRService<TradeEvent, TradeEventRequest> _tradeEventService;
        private ISignalRService<Price, PriceRequest> _priceEventService;

        public TradeService()
        {
            _repository = new List<ITrade>();

            for (var i = 0; i < 10; i++)
            {
                var trade = TradeServiceReferential.GenerateTrade();
                _repository.Add(trade);
            }

            _tradeEventService = SignalRServiceBuilder<TradeEvent, TradeEventRequest>
                                .Create()
                                .Build(new TradeEventRequest((p) => true));

            _tradeEventService.Connect(Scheduler.Default, 1000)
                               .Subscribe((tradeEvent) =>
                               {
                                   var trade = _repository.FirstOrDefault(t => t.Id == tradeEvent.TradeId);
                                   if (null == trade) return;
                                   trade.Status = tradeEvent.Status;
                               });

            _priceEventService = SignalRServiceBuilder<Price, PriceRequest>
                                            .Create()
                                            .Build(new PriceRequest((p) => true));


        }

        public Task<TradeCreationResult> CreateTrade(TradeCreationRequest request)
        {
            var trade = new Trade(Guid.NewGuid(), DateTime.Now, request.Counterparty, request.Asset, TradeStatus.None, request.Way, request.Price, request.Volume);
            _repository.Add(trade);

            var result = new TradeCreationResult()
            {
                TradeId = trade.Id,
                TradeStatus = TradeStatus.Created
            };

            return Task.FromResult(result);

        }

        public Task<IEnumerable<ITrade>> GetAllTrades()
        {
            return Task.FromResult(_repository.Cast<ITrade>());
        }

        public Task<IEnumerable<ITrade>> GetAllTradesViaCache()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ITrade>> GetAllTradesViaMiddleware()
        {
            throw new NotImplementedException();
        }

        public Task<ITrade> GetTradeById(Guid tradeId)
        {
            return Task.FromResult(_repository.FirstOrDefault(trade => trade.Id == tradeId));
        }
    }
}
