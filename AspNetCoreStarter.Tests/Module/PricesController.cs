using AspNetCoreStarter.Tests.Domain;
using AspNetCoreStarterPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Tests.Module
{
    public class PricesController : ServiceControllerBase
    {
        private IPriceService _priceService;

        public PricesController(IPriceService priceService, JsonSerializerSettings settings)
        {
            _priceService = priceService;
        }

        [Authorize(TradeReferential.EquityTraderUserPolicy)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IPrice>>> GetPrices()
        {
            return Ok(await _priceService.GetPrices());
        }


    }
}
