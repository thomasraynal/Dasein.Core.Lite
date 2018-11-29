using AspNetCoreStarterPack.Infrastructure;
using AspNetCoreStarterPack.SignalR;
using GraphQL.Builders;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace AspNetCoreStarterPack.Default
{
    public class ConnectionProvider : IConnectionProvider 
    {
        private IEnumerable<string> _servers;
        private readonly IHubRequestFilter _request;
        private readonly String _hubName;
        private Func<HubConnectionBuilder> _getBuilder;
        private int _currentIndex;
        
        public ConnectionProvider(HubDescriptor config, IHubRequestFilter request, Func<HubConnectionBuilder> getBuilder)
        {
            _servers = config.Endpoints;
            _servers.Shuffle();
            _request = request;
            _hubName = config.Name;
            _getBuilder = getBuilder;
        }

        private String Next(String current)
        {
            var endpoint = _servers.ElementAt(_currentIndex++);
            
            if (_currentIndex == _servers.Count())
            {
                _currentIndex = 0;
            }

            if (_servers.Count() == 1 || null == current) return endpoint;
            
            if (current == endpoint) return Next(current);

            return endpoint;

        }

        public IServiceConfiguration Configuration
        {
            get
            {
                return AppCore.Instance.Get<IServiceConfiguration>();
            }
        }

        public IServiceConnection GetNextConnection(Action onError, Action onSuccess, String endpoint = null)
        {
            var next= Next(endpoint);

            var connection = _getBuilder()
                    .WithQuery(next,_request)
                    .Build();
            
            return new Connection(next, connection, onError, onSuccess);
        }

    }
}
