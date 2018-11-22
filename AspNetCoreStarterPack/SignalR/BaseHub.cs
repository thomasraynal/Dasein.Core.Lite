using AspNetCoreStarterPack.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarterPack.SignalR
{
    public abstract class BaseHub<TDto, TRequest> : Hub, ICanLog, IDisposable where TRequest : IHubRequest<TDto>
    {
        private readonly IHubContextHolder _context;

        private readonly Dictionary<Type, List<String>> _connectedIds;

        public abstract String Name { get; }

        public BaseHub(IHubContextHolder context)
        {
            _context = context;
            _connectedIds = new Dictionary<Type, List<String>>();
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
            _context.RegisterUserId(Name, Context.ConnectionId);

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
