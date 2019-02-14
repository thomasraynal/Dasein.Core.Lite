using GraphQL.Builders;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Dasein.Core.Lite.Shared
{
    public class ConnectionProvider : IConnectionProvider 
    {
        private IEnumerable<string> _servers;
        private readonly IHubRequestFilter _request;
        private readonly String _hubName;
        private Func<HubConnectionBuilder> _getBuilder;
        private int _currentIndex;
        private readonly Action<HttpConnectionOptions> _httpConnectionOptions;
        private readonly HttpTransportType _transportType;


        public ConnectionProvider(HubDescriptor config, IHubRequestFilter request, HttpTransportType transports, Action<HttpConnectionOptions> configureHttpConnection, Func<HubConnectionBuilder> getBuilder)
        {
            _servers = config.Endpoints;
            _servers.Shuffle();
            _request = request;
            _hubName = config.Name;
            _httpConnectionOptions = configureHttpConnection;
            _transportType = transports;
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

        public IServiceConnection GetNextConnection(Action onError, Action onSuccess, String endpoint = null)
        {
            var next = Next(endpoint);

            var builder = _getBuilder();

            var opts = new HttpConnectionOptions();

            _httpConnectionOptions(opts);

            var connection = builder
                    .WithQuery(next, _request, _transportType, _httpConnectionOptions)
                    .Build();


            return new Connection(next, connection, onError, onSuccess);
        }

    }
}
