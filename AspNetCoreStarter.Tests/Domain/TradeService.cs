using AspNetCoreStarterPack.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Tests.Domain
{
    public class TradeService : ITradeService, ICanLog
    {
        private readonly List<ITrade> _repository;

        public TradeService()
        {
            _repository = new List<ITrade>();

            for (var i = 0; i < 10; i++)
            {
                var trade = TradeReferential.GenerateTrade();
                _repository.Add(trade);
            }
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

        public Task<ITrade> GetTradeById(Guid tradeId)
        {
            return Task.FromResult(_repository.FirstOrDefault(trade => trade.Id == tradeId));
        }
    }
}
