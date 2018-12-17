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
        public string Key { get; set; }
        public long CacheDuration { get; set; }
        public long TokenExpiration { get; set; }

        public T GetServiceConfigurationValue<T>(string key)
        {
            return Root.GetValue<T>($"{ServiceConstants.serviceConfiguration}:{key}");
        }
    }
}
