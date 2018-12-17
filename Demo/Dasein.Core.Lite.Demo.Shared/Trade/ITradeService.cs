using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Demo.Shared
{
    public interface ITradeService
    {
        [Put("/api/v1/trade")]
        Task<TradeCreationResult> CreateTrade(TradeCreationRequest request);
        [Get("/api/v1/trade")]
        Task<IEnumerable<ITrade>> GetAllTrades();
        [Get("/api/v1/trade/{tradeId}")]
        Task<ITrade> GetTradeById(Guid tradeId);
        [Get("/api/v1/trade/middleware")]
        Task<IEnumerable<ITrade>> GetAllTradesViaMiddleware();
        [Get("/api/v1/trade/cached")]
        Task<IEnumerable<ITrade>> GetAllTradesViaCache();
    }
}
