using Dasein.Core.Lite.Demo.Server;
using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite;
using Dasein.Core.Lite.Shared;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Demo.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new Host<TradeServiceStartup>();

            var app = host.Build(args);

            app.Start();
            app.WaitForShutdown();
        }
    }
}
