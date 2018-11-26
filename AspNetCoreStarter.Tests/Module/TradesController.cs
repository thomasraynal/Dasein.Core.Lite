using AspNetCoreStarter.Tests.Domain;
using AspNetCoreStarterPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Tests.Module
{
    public class TradesController : ServiceControllerBase
    {
        private ITradeService _tradeService;

        public TradesController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [Authorize(TradeReferential.TraderUserPolicy)]
        [HttpPut]
        public async Task<ActionResult<TradeCreationResult>> CreateTrade([FromBody] TradeCreationRequest request)
        {
            var tradeResult = await _tradeService.CreateTrade(request);
            return CreatedAtAction(nameof(GetTradeById), new { tradeId = tradeResult.TradeId }, tradeResult);
        }

        [HttpGet]
        public async Task<IEnumerable<ITrade>> GetAllTrades()
        {
            return await _tradeService.GetAllTrades();
        }

        [HttpGet("{tradeId}")]
        public async Task<ITrade> GetTradeById([FromQuery] Guid tradeId)
        {
            return await _tradeService.GetTradeById(tradeId);
        }
    }
}
