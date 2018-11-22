using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.SignalR
{
    public interface IHubContextHolder
    {
        void RegisterUserId(string hub, string userId);
        void UnRegisterUserId(string hub, string userId);
    }
}
