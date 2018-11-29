using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.Cache
{
    public class Cached : Attribute
    {
        public Cached(long duration)
        {
            Duration = duration;
        }

        public long Duration { get; set; }
    }
}
