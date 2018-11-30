using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public interface ICachedRessource
    {
        IEnumerable<String> GetCacheInvalidationTags();
        int GetCacheKey();
    }
}
