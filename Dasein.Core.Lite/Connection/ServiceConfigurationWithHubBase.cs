using Dasein.Core.Lite.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite
{ 
    public abstract class ServiceHubConfigurationBase : ServiceConfigurationBase, IHubConfiguration
    {
        public HubDescriptor[] Hubs { get; set; }
    }
}
