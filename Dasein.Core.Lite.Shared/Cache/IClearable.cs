using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public interface IClearable
    {
        CacheStatus GetStatus();
        Task Clear();
        Task InvalidateWhenKeyContains(String key);
    }
}
