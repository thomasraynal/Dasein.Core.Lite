using AspNetCoreStarterPack.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Tests.Domain
{
    public interface IPriceService
    {
        [Cached(3000)]
        Task<IEnumerable<IPrice>> GetAllPrices();

        Task CreatePrice(IPrice price);

        Task<IEnumerable<IPrice>> GetPricesNoCache();
        Task<IEnumerable<IPrice>> GetPricesByAsset(string asset);
        Task<IEnumerable<IPrice>> GetPricesByTrade(Guid tradeId);
    }
}
