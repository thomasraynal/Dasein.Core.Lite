using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.Default
{
    public abstract class ServiceHubConfigurationBase : ServiceConfigurationBase, IHubConfiguration
    {
        public HubDescriptor[] Hubs { get; set; }
    }
}
