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
    public class PriceController : ServiceControllerBase
    {
        private IPriceService _priceService;

        public PriceController(IPriceService priceService, JsonSerializerSettings settings)
        {
            _priceService = priceService;
        }

        [Authorize(TradeReferential.EquityTraderUserPolicy)]
        [HttpGet]
        public async Task<IEnumerable<IPrice>> GetPrices(bool cache = true)
        {
            if (cache) return await _priceService.GetAllPrices();

            return await _priceService.GetPricesNoCache();
        }
    }
}
