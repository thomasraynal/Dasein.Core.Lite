using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.Cache
{
    public class CacheStatus
    {
        public CacheType Type { get; set; }
        public double Size { get; set; }
        public String Name { get; set; }
    }
}
