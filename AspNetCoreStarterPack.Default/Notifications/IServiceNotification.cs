using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarterPack.Default
{
    public interface IServiceNotification
    {
        String Subject { get; }
        String Reason { get; }
        String Message { get; }
        object Payload { get; }
    }
}
