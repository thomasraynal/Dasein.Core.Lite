using AspNetCoreStarterPack;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Security.Claims;

namespace AspNetCoreStarter.Tests
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
