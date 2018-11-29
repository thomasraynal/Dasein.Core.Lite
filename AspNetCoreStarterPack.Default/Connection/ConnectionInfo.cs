using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarterPack.Default
{
    public class ConnectionInfo
    {
        public ConnectionStatus ConnectionStatus { get; private set; }
        public string Endpoint { get; private set; }

        public ConnectionInfo(ConnectionStatus connectionStatus, string server)
        {
            ConnectionStatus = connectionStatus;
            Endpoint = server;
        }

        public override string ToString()
        {
            return $"ConnectionStatus: {ConnectionStatus}, Server: {Endpoint}";
        }
    }
}
