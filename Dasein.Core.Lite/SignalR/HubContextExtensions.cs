using Dasein.Core.Lite.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite
{
    public static class HubContextExtensions
    {
        public static async Task RaiseChange<THub, TDto>(this IHubContext<THub> hubContext, TDto change) where THub : HubBase<TDto>
        {
            var context = AppCore.Instance.Get<IHubContextHolder<TDto>>();

            foreach (var connection in context.Groups)
            {
                if (connection.Value(change)) await hubContext.Clients.Client(connection.Key).SendAsync(SignalRConstants.OnUpdate, change);
            }
        }

        public static async Task RaiseChange<TDto>(this HubConnection connection, TDto change)
        {
            if (connection.State == HubConnectionState.Disconnected) return;

            await connection.InvokeAsync(SignalRConstants.RaiseChange, change);
        }

    }
}
