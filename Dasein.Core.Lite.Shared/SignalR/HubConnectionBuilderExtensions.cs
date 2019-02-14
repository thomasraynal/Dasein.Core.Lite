using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public static class HubConnectionBuilderExtensions
    {

        public static IHubConnectionBuilder WithQuery(this IHubConnectionBuilder hubConnectionBuilder, string url, IHubRequestFilter filter)
        {
            return hubConnectionBuilder.WithUrl($"{url}?{HubConstants.HubQueryFilter}={filter.GroupId}");
        }

        public static IHubConnectionBuilder WithQuery(this IHubConnectionBuilder hubConnectionBuilder, string url, IHubRequestFilter filter, Action<HttpConnectionOptions> configureHttpConnection)
        {
            return hubConnectionBuilder.WithUrl($"{url}?{HubConstants.HubQueryFilter}={filter.GroupId}", configureHttpConnection);
        }

        public static IHubConnectionBuilder WithQuery(this IHubConnectionBuilder hubConnectionBuilder, string url, IHubRequestFilter filter, HttpTransportType transports)
        {
            return hubConnectionBuilder.WithUrl($"{url}?{HubConstants.HubQueryFilter}={filter.GroupId}", transports);
        }

        public static IHubConnectionBuilder WithQuery(this IHubConnectionBuilder hubConnectionBuilder, string url, IHubRequestFilter filter, HttpTransportType transports, Action<HttpConnectionOptions> configureHttpConnection)
        {
            return hubConnectionBuilder.WithUrl($"{url}?{HubConstants.HubQueryFilter}={filter.GroupId}", transports, configureHttpConnection);
        }

    }
}
