using AspNetCoreStarterPack;
using AspNetCoreStarterPack.Default;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Tests
{
    public class TradeServiceConfiguration : ServiceHubConfigurationBase
    {
        public override string Name { get; set; }
        public override int Version { get; set; }
        public string Key { get; set; }
    }
}
