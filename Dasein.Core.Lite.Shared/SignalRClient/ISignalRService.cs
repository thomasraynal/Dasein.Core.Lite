using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Reactive.Concurrency;

namespace Dasein.Core.Lite.Shared
{
    public interface ISignalRService<TDto, TRequest> : IDisposable
            where TRequest : IHubRequest<TDto>
    {
        IServiceConnection Current { get; }
        IConnectionProvider ConnectionProvider { get; }
        Func<HubConnectionBuilder> ConnectionBuilderProvider { get; }
        IObservable<bool> IsActive { get; }
        string HubName { get; }
        string OnStreamUpdateMethodName { get; }
        TRequest Request { get; }
        IObservable<TDto> Connect(IScheduler scheduler, long connectionTimeoutDelay);
        void Disconnect();
        void BuildInternal();
    }
}
