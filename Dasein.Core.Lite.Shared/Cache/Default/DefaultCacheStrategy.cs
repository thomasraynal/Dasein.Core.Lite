using Dasein.Core.Lite.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public class DefaultCacheStrategy<TObject> : ICacheStrategy<TObject>
    {
        private ICacheProvider<TObject> _persistantCache;
        private ICacheProvider<TObject> _memoryCache;

        public DefaultCacheStrategy(IServiceConfiguration configuration)
        {
            _persistantCache = null;
            _memoryCache = new DefaultMemoryCache<TObject>(configuration.Name);
        }

        public ICacheProvider<TObject> PersistantCache => _persistantCache;

        public ICacheProvider<TObject> MemoryCache => _memoryCache;

        public async Task Clear(CacheType cacheType)
        {
            if (cacheType == CacheType.Volatile) await MemoryCache.Clear();
            if (cacheType == CacheType.Persistant) await PersistantCache.Clear();
        }

        public async Task Clear(CacheType cacheType, string key)
        {
            if (cacheType == CacheType.Volatile) await MemoryCache.Remove(key);
            if (cacheType == CacheType.Persistant) await PersistantCache.Remove(key);
        }

        public async Task Clear()
        {
            await MemoryCache.Clear();
            await PersistantCache.Clear();
        }

        public IEnumerable<CacheStatus> GetStatus()
        {
            yield return _memoryCache.GetStatus();
            yield return _persistantCache.GetStatus();
        }

        public async Task InvalidateWhenKeyContains(string key)
        {
            await MemoryCache.InvalidateWhenKeyContains(key);
        }

    }
}
