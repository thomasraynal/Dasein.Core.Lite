using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.SignalR
{
    public class HubRequestKey
    {
        public HubRequestKey(HubRequestFilter filter)
        {

        }

        public string Key { get; private set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
