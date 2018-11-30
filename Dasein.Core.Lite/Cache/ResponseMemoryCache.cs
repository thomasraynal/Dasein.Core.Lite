using Dasein.Core.Lite.Shared;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite
{
    public class ResponseMemoryCache : IMemoryCache
    {
        private ICacheProvider<ResponseCacheEntry   > _memcache;

        public ResponseMemoryCache(ICacheStrategy<ResponseCacheEntry> cache)
        {
            _memcache = cache.MemoryCache;
        }

        public ICacheEntry CreateEntry(object key)
        {
            return new ResponseCacheEntry()
            {
                Key = key
            };
        }
        
        public void Dispose()
        {
            return;
        }

        public void Remove(object key)
        {
            _memcache.Remove(key.GetHashCode().ToString());
        }

        public bool TryGetValue(object key, out object value)
        {
            value = _memcache.Get(key.GetHashCode().ToString());
            return null != value;
        }
    }
}
