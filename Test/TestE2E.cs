//using AspNetCoreStarter.Tests;
//using AspNetCoreStarter.Tests.Domain;
//using AspNetCoreStarterPack;
//using AspNetCoreStarterPack.Infrastructure;
//using Microsoft.AspNetCore.Hosting;
using AspNetCoreStarter.Tests;
using AspNetCoreStarter.Tests.Domain;
using AspNetCoreStarter.Tests.Module;
using AspNetCoreStarterPack;
using AspNetCoreStarterPack.Infrastructure;
using AspNetCoreStarterPack.SignalR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TestFixture]
    public class TestE2E
    {
        private Host<TradeServiceStartup> _host;
        private IWebHost _app;
        private IServiceConfiguration _configuration;

        [OneTimeSetUp]
        public void SetUp()
        {
            _host = new Host<TradeServiceStartup>();
            _app = _host.Build();
            _app.Start();

            _configuration = AppCore.Instance.Get<IServiceConfiguration>();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _app.StopAsync();
        }


        [Test]
        public async Task TestSignalRClient()
        {
            var query = new PriceRequest((p) => p.Value > 50);

            var connection = new HubConnectionBuilder()
                 .WithQuery(_configuration.Root["urls"], query)
                 .Build();

            await connection.StartAsync();

            connection.On<Price>(TradeReferential.OnPriceChanged, (p) =>
             {
                 Assert.AreEqual("stock3", p.Asset);
                 Assert.AreEqual(60, p.Value);
             });

            await connection.InvokeAsync(TradeReferential.RaisePriceChanged, new Price(Guid.NewGuid(), "stock1", 20, DateTime.Now));
            await connection.InvokeAsync(TradeReferential.RaisePriceChanged, new Price(Guid.NewGuid(), "stock2", 30, DateTime.Now));
            await connection.InvokeAsync(TradeReferential.RaisePriceChanged, new Price(Guid.NewGuid(), "stock3", 60, DateTime.Now));

            await Task.Delay(500);

        }

    }
}
