using AspNetCoreStarter.Demo.Common.Domain;
using AspNetCoreStarter.Tests;
using AspNetCoreStarterPack;
using AspNetCoreStarterPack.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Demo.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new Host<TradeServiceStartup>();

            var app = host.Build(args);

            app.Start();

            Task.Delay(500).ContinueWith(async (_) =>
            {
                var publishers = AppCore.Instance.GetAll<IPublisher>();

                foreach (var publisher in publishers)
                {
                    await publisher.Start();
                }

            });

            app.WaitForShutdown();
        }
    }
}
