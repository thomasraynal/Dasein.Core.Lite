using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.Default
{
    public class HubDescriptor
    {
        public String Name { get; set; }
        public String[] Endpoints { get; set; }
    }
}
