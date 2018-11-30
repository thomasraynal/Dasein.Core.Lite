using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite
{
    public class ResponseCacheEntry: ICacheEntry
    {
        public object Key { get; set; }
        public object Value { get; set; }
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }

        public IList<IChangeToken> ExpirationTokens { get; }

        public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; }

        public CacheItemPriority Priority { get; set; }
        public long? Size { get; set; }

        public void Dispose()
        {
            return;
        }
    }
}
