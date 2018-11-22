using AspNetCoreStarterPack.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Tests.Domain
{
    public class PriceService : IPriceService
    {
        private Task<IEnumerable<IPrice>> GetPricesInternal()
        {
            var prices = Enumerable.Range(0, 50)
                 .Select(_ => new Price(TradeReferential.Assets.Random().Name, TradeReferential.Rand.NextDouble()))
                 .GroupBy(price => price.Asset)
                 .Where(group => group.Count() == 1)
                 .SelectMany(group => group)
                 .Cast<IPrice>()
                 .ToList();

            return Task.FromResult(prices.AsEnumerable());
        }

        public async Task<IEnumerable<IPrice>> GetPrices()
        {
            var prices = await GetPricesInternal();
            return prices;
        }

        public async Task<IEnumerable<IPrice>> GetPricesNoCache()
        {
            var prices = await GetPricesInternal();
            return prices;
        }
    }
}
