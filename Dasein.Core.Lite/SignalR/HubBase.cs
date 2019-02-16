using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using Dasein.Core.Lite.Shared;

namespace Dasein.Core.Lite
{
    public abstract class HubBase<TDto> : Hub, ICanLog, IDisposable
    {
        private readonly IHubContextHolder<TDto> _context;

        public abstract String Name { get; }


        public HubBase(IHubContextHolder<TDto> context)
        {
            _context = context;
        }

        public Task RaiseChange(TDto change)
        {
            foreach (var connection in _context.Groups)
            {
                if (connection.Value(change)) this.Clients.Client(connection.Key).SendAsync(SignalRConstants.OnUpdate, change);
            }

            return Task.CompletedTask;
        }

        protected virtual Task OnConnectedAsyncInternal()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnDisconnectedInternal()
        {
            return Task.CompletedTask;
        }

        public async override Task OnConnectedAsync()
        {

            var httpContext = Context.GetHttpContext();
            var filter = httpContext.Request.Query[HubConstants.HubQueryFilter].FirstOrDefault();

            var query = _context.AcceptAll;

            if (null != filter)
            {
                query = (HubRequestFilter.FromBase64(filter).FilterExpression as Expression<Func<TDto, bool>>).Compile();
            }

            _context.RegisterUserId(Name, Context.ConnectionId, query);

            await OnConnectedAsyncInternal();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            if (null != exception) this.LogError(exception);

            _context.UnRegisterUserId(Name, Context.ConnectionId);

            await OnDisconnectedInternal();

        }
    }
}
