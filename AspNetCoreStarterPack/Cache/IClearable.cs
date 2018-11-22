using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarterPack.Cache
{
    public interface IClearable
    {
        CacheStatus GetStatus();
        Task Clear();
        Task InvalidateWhenKeyContains(String key);
    }
}
