using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Connections.Client;
using AspNetCoreStarterPack.Infrastructure;
using AspNetCoreStarterPack.Default;
using Newtonsoft.Json;

namespace AspNetCoreStarterPack.Default
{
    public class Connection : IServiceConnection, ICanLog
    {
        private ISubject<ConnectionInfo> _statusStream;
        private bool _initialized;
        private IDisposable _signalrConnectionChanged;
        private IDisposable _statusChanged;

        public ConnectionStatus CurrentState { get; private set; }

        public HubConnection Proxy { get; private set; }

        public Connection(string endpoint, HubConnection proxy, Action onError, Action onSuccess)
        {
            Endpoint = endpoint;
            Proxy = proxy;

            _statusStream = new BehaviorSubject<ConnectionInfo>(new ConnectionInfo(ConnectionStatus.Uninitialized, Endpoint));

            _statusChanged = _statusStream.Subscribe((change) =>
            {
                CurrentState = change.ConnectionStatus;
            });

            var onErrorHandler = new Func<Exception, Task>((ex) =>
            {

                onError();

                _statusStream.OnNext(new ConnectionInfo(ConnectionStatus.Closed, Endpoint));

                _signalrConnectionChanged.Dispose();
                _statusChanged.Dispose();

                return Task.CompletedTask;
            });

            _signalrConnectionChanged = Observable.FromEvent(h => Proxy.Closed += onErrorHandler, h => Proxy.Closed -= onErrorHandler).Subscribe();

        }

        private async Task<bool> StartConnection()
        {
            try
            {
                this.LogInformation($"Connecting to [{Endpoint}]");
                await Proxy.StartAsync();
                _statusStream.OnNext(new ConnectionInfo(ConnectionStatus.Connected, Endpoint));
                return true;
            }
            catch (Exception ex)
            {
                _statusStream.OnNext(new ConnectionInfo(ConnectionStatus.Closed, Endpoint));
                this.LogInformation($"Failed to connect to [{Endpoint}]");
                this.LogError($"An error occurred when starting connection to [{Endpoint}]", ex);
                return false;
            }
        }

        public async Task Stop()
        {
            try
            {
                this.LogInformation($"Stopping connection [{Endpoint}]");
                await Proxy.StopAsync();
                this.LogInformation($"Connection stopped [{Endpoint}]");
            }
            catch (Exception e)
            {
                this.LogError($"An error occurred while stoping connection [{Endpoint}]", e);
            }
        }

        public async Task<bool> Initialize()
        {
            if (_initialized)
            {
                throw new InvalidOperationException("Connection has already been initialized");
            }
            _initialized = true;

            _statusStream.OnNext(new ConnectionInfo(ConnectionStatus.Connecting, Endpoint));

            return await StartConnection();

        }

        public IObservable<ConnectionInfo> StatusStream
        {
            get { return _statusStream; }
        }

        public string Endpoint { get; private set; }

        public override string ToString()
        {
            return string.Format($"Address: [{Endpoint}]");
        }
    }
}