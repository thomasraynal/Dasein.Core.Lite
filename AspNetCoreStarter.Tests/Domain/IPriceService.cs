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
        Task<IEnumerable<IPrice>> GetPrices();

        Task<IEnumerable<IPrice>> GetPricesNoCache();
    }
}
