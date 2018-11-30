using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public interface ICacheProvider<TObject> : IClearable
    {
        Task Put(string key, TObject obj);
        Task<IEnumerable<TObject>> GetAllObjects();
        Task<IEnumerable<String>> GetAllKeys();
        Task<TObject> Get(string key);
        Task<TObject> GetOrSet(String cacheKey, Func<Task<TObject>> action);
        Task Remove(string key);
        CacheType CacheType { get; }
        Task<bool> Exists(string key);
    }
}
