using System;
using System.Collections.Generic;
using System.Text;
using Dasein.Core.Lite.Shared;
using Microsoft.Extensions.Configuration;

namespace Dasein.Core.Lite
{
    public abstract class ServiceConfigurationBase : IServiceConfiguration
    {
        public IAppContainer Container { get; internal set; }
        public IConfiguration Root { get; internal set; }
        public abstract string Name { get; set; }
        public abstract int Version { get; set; }
    }
}
