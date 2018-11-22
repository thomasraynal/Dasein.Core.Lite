using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Tests.Domain
{
    public interface ITradeService
    {
        Task<TradeCreationResult> CreateTrade(TradeCreationRequest request);
        Task<IEnumerable<ITrade>> GetAllTrades();
        Task<ITrade> GetTradeById(Guid tradeId);
    }
}
