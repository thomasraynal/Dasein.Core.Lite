using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreStarterPack
{
    public abstract class ServiceConfigurationBase : IServiceConfiguration
    {
        public IAppContainer Container { get; internal set; }
        public IConfiguration Root { get; internal set; }
        public abstract string Name { get; set; }
        public abstract int Version { get; set; }

        
    }
}
