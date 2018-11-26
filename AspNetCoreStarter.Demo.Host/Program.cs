using AspNetCoreStarter.Tests;
using AspNetCoreStarterPack;
using Microsoft.AspNetCore.Hosting;
using System;

namespace AspNetCoreStarter.Demo.Host
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
