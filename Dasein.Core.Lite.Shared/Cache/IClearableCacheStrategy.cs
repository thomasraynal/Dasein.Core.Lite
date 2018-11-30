using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public interface IClearableCacheStrategy
    {
        IEnumerable<CacheStatus> GetStatus();
        Task InvalidateWhenKeyContains(string key);
        Task Clear();
    }
}
