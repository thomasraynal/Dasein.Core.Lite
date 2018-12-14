﻿using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Demo.Server
{
    public class TradeController : ServiceControllerBase<ITradeService>
    {

        [Authorize(TradeServiceReferential.EquityTraderUserPolicy)]
        [HttpPut]
        public async Task<ActionResult<TradeCreationResult>> CreateTrade([FromBody] TradeCreationRequest request)
        {
            var tradeResult = await Service.CreateTrade(request);
            return CreatedAtAction(nameof(GetTradeById), new { tradeId = tradeResult.TradeId }, tradeResult);
        }

        [Authorize(TradeServiceReferential.TraderUserPolicy)]
        [HttpGet]
        public async Task<IEnumerable<ITrade>> GetAllTrades()
        {
            return await Service.GetAllTrades();
        }

        [Authorize(TradeServiceReferential.TraderUserPolicy)]
        [HttpGet("{tradeId}")]
        public async Task<ITrade> GetTradeById([FromRoute] Guid tradeId)
        {
            return await Service.GetTradeById(tradeId);
        }
    }
}
