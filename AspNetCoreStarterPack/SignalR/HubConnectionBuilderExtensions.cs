using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.SignalR
{
    public static class HubConnectionBuilderExtensions
    {
        public static IHubConnectionBuilder WithQuery(this IHubConnectionBuilder builder, string root, IHubRequestFilter filter)
        {
            return builder.WithUrl($"{root}?{HubConstants.HubQueryFilter}={filter.GroupId}");
        }
    }
}
