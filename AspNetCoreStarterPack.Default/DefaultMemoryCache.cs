using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Caching;
using AspNetCoreStarterPack.Cache;

namespace AspNetCoreStarterPack.Default
{
    public class DefaultMemoryCache<TObject> : ICacheProvider<TObject>
    {
        private readonly string _applicationName;
        private MemoryCache _cache;

        private readonly TimeSpan cacheDuration = new TimeSpan(0, 30, 0);

        public CacheType CacheType => CacheType.Volatile;

        public DefaultMemoryCache(String applicationName)
        {
            _applicationName = applicationName;
            _cache = new MemoryCache(applicationName);
        }

        public Task<TObject> Get(string key)
        {
            var result = _cache.Get(key);
            TObject @object;

            if (null == result) @object = default(TObject);
            else
            {
                @object = (TObject)result;
            }

            return Task.FromResult(@object);
        }

        public async Task<TObject> GetOrSet(string key, Func<Task<TObject>> action)
        {
            var @object = await Get(key);

            if (null != @object) return @object;

            @object = await action();

            await Put(key, @object);

            return @object;
        }

        public Task Remove(string key)
        {
            _cache.Remove(key);

            return Task.CompletedTask;
        }

        public Task Put(string key, TObject obj)
        {
            _cache.Set(key, obj, new DateTimeOffset(DateTime.Now.Ticks, cacheDuration));

            return Task.CompletedTask;
        }

        public CacheStatus GetStatus()
        {
            var status = new CacheStatus()
            {
                Size = _cache.Count(),
                Name = _cache.Name,
                Type = CacheType.Volatile
            };

            return status;
        }

        public Task Clear()
        {
            _cache = new MemoryCache(_applicationName);

            return Task.CompletedTask;
        }

        public async Task InvalidateWhenKeyContains(string key)
        {
            foreach (var keyValue in _cache)
            {
                if (keyValue.Key.ToInvariant().Contains(key.ToInvariant()))
                {
                    await Remove(keyValue.Key);
                }
            }
        }

        public Task<bool> Exists(string key)
        {
            var doesKeyExist = _cache.Contains(key);
            return Task.FromResult(doesKeyExist);
        }

        public Task<IEnumerable<string>> GetAllKeys()
        {
            var keys = _cache.Select(kv => kv.Key);
            return Task.FromResult(keys);
        }

        public Task<IEnumerable<TObject>> GetAllObjects()
        {
            var objects = _cache.Select(kv => (TObject)kv.Value);
            return Task.FromResult(objects);
        }
    }
}

