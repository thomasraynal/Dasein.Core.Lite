using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public interface ICacheStrategy<TObject> : IClearableCacheStrategy
    {
        ICacheProvider<TObject> PersistantCache { get; }
        ICacheProvider<TObject> MemoryCache { get; }
    }
}
