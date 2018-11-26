using AspNetCoreStarter.Tests.Module;
using AspNetCoreStarterPack;
using AspNetCoreStarterPack.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Tests.Domain
{
    //public class PriceHubClient<TDto, TRequest> : IDisposable 
    //    where TRequest: IHubRequest<TDto>
    //{
    //    private HubConnection _connection;
    //    private IHubProxy _hubProxy;
    //    private IDisposable _feed;
    //    private IServiceConfiguration _configuration;

    //    public async Task Start(TRequest request)
    //    {
    //        _connection = new HubConnection($"{request.GroupId}?{HubConstants.HubQueryFilter}={_configuration.Root["urls"]}");
    //        _hubProxy = _connection.CreateHubProxy(nameof(PriceHub));
    //        _feed = _hubProxy.On<IPrice>(TradeReferential.PriceUpdate, stock => Console.WriteLine("Stock update for {0} new price {1}", stock.Asset, stock.Value));
    //        await _connection.Start();
    //    }

    //    public void Dispose()
    //    {
    //        _feed.Dispose();
    //    }

    //    public PriceHubClient(IServiceConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }
    //}
}
