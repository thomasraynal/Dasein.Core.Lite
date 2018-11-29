using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.Cache
{
    public class ResponseMemoryCache : IMemoryCache
    {

        public ResponseMemoryCache(ICacheStrategy<ResponseCacheEntry> cache)
        {

        }

        public ICacheEntry CreateEntry(object key)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            return;
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(object key, out object value)
        {
            throw new NotImplementedException();
        }
    }
}
