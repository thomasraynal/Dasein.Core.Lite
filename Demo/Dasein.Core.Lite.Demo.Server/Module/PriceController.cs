using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Demo.Server
{
    public class PriceController : ServiceControllerBase
    {
        private IPriceService _priceService;

        public PriceController(IPriceService priceService, JsonSerializerSettings settings)
        {
            _priceService = priceService;
        }

        [Authorize(TradeServiceReferential.EquityTraderUserPolicy)]
        [HttpGet]
        public async Task<IEnumerable<IPrice>> GetPrices(bool cache = true)
        {
            if (cache) return await _priceService.GetAllPrices();

            return await _priceService.GetPricesNoCache();
        }
    }
}
