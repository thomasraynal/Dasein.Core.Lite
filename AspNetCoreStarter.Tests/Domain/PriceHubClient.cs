using AspNetCoreStarter.Tests.Module;
using AspNetCoreStarterPack;
using AspNetCoreStarterPack.SignalR;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Tests.Domain
{
    public class PriceHubClient<TRequest, TDto> : IDisposable 
        where TRequest: IHubRequest<TDto>
    {
        private HubConnection _connection;
        private IHubProxy _hubProxy;
        private IDisposable _feed;

        public async Task Start()
        {
            await _connection.Start();
        }

        public void Dispose()
        {
            _feed.Dispose();
        }

        public PriceHubClient(IServiceConfiguration configuration)
        {
            _connection = new HubConnection(configuration.Root["urls"]);
            _hubProxy = _connection.CreateHubProxy(nameof(PriceHub));
            _feed = _hubProxy.On<IPrice>("UpdateStockPrice", stock => Console.WriteLine("Stock update for {0} new price {1}", stock.Asset, stock.Value));
        }
    }
}
