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

        public long Duration { get; set; }
    }
}
