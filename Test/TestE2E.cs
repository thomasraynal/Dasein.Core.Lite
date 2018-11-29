//using AspNetCoreStarter.Tests;
//using AspNetCoreStarter.Tests.Domain;
//using AspNetCoreStarterPack;
//using AspNetCoreStarterPack.Infrastructure;
//using Microsoft.AspNetCore.Hosting;
using AspNetCoreStarter.Tests;
using AspNetCoreStarter.Tests.Domain;
using AspNetCoreStarter.Tests.Module;
using AspNetCoreStarterPack;
using AspNetCoreStarterPack.Default;
using AspNetCoreStarterPack.Infrastructure;
using AspNetCoreStarterPack.SignalR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using NUnit.Framework;
using System;
using System.Reactive.Concurrency;
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

            await connection.DisposeAsync();
        }

        [Test]
        public async Task TestSignalRClientResilientConnection()
        {
            var query = new PriceRequest((p) => p.Value > 50);

            var service = SignalRServiceBuilder<Price, PriceRequest>
                            .Create()
                            .Build(query);

            var disposable = service.Connect(Scheduler.Default, 0)
                    .Subscribe(p =>
                    {
                        Assert.AreEqual("stock3", p.Asset);
                        Assert.AreEqual(60, p.Value);
                    });

            await Task.Delay(100);

            while (service.Current.CurrentState != ConnectionStatus.Connected)
            {
                await Task.Delay(500);
            }

            await service.Current.Proxy.InvokeAsync(TradeReferential.RaisePriceChanged, new Price(Guid.NewGuid(), "stock1", 20, DateTime.Now));
            await service.Current.Proxy.InvokeAsync(TradeReferential.RaisePriceChanged, new Price(Guid.NewGuid(), "stock2", 30, DateTime.Now));
            await service.Current.Proxy.InvokeAsync(TradeReferential.RaisePriceChanged, new Price(Guid.NewGuid(), "stock3", 60, DateTime.Now));


            await Task.Delay(500);

            disposable.Dispose();
            service.Disconnect();
        }
    }
}
