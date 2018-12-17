using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class Cached : Attribute
    {
        public Cached(long duration)
        {
            Duration = duration;
        }

        public Cached()
        {
            Duration = AppCore.Instance.Get<IServiceConfiguration>().CacheDuration;
        }

        public long Duration { get; set; }
        public bool IsUserBounded { get; set; }
    }
}
