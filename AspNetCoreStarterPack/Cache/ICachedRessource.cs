using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.Cache
{
    public interface ICachedRessource
    {
        IEnumerable<String> GetCacheInvalidationTags();
        int GetCacheKey();
    }
}
