using Dasein.Core.Lite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Demo.Server
{
    public class TradeServiceConfiguration : ServiceHubConfigurationBase
    {
        public override string Name { get; set; }
        public override int Version { get; set; }
        public string Key { get; set; }
    }
}
