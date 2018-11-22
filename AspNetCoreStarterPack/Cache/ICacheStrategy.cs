using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarterPack.Cache
{
    public interface ICacheStrategy<TObject> : IClearableCacheStrategy
    {
        ICacheProvider<TObject> PersistantCache { get; }
        ICacheProvider<TObject> MemoryCache { get; }
    }
}
