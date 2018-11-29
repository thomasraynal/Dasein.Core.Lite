using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.Default
{
    public interface IHubConfiguration
    {
        HubDescriptor[] Hubs { get; }
    }
}
