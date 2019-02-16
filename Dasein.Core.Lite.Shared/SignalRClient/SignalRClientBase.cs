using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;

namespace Dasein.Core.Lite.Shared
{
    public abstract class SignalRServiceClientBase<TDto, TRequest> : ISignalRService<TDto, TRequest>
              where TRequest : IHubRequest<TDto>
    {
        private ISubject<TDto> _resilientStream;
        private ISubject<bool> _activitySubject;
        private CancellationTokenSource _doCancel;
        private bool _isServiceActive;
        private CancellationToken _cancel;
        private IDisposable _resilientStreamProcess;
        private Action _onError;
        private Action _onSuccess;

        public abstract String HubName { get; }
        public string OnStreamUpdateMethodName => SignalRConstants.OnUpdate;
        public TRequest Request { get; }
        public Action<HttpConnectionOptions> HttpConnectionOptions { get; }
        public HttpTransportType TransportType { get; }
        public IObservable<bool> IsActive { get; private set; }
        public IConnectionProvider ConnectionProvider { get; private set; }
        public abstract Func<HubConnectionBuilder> ConnectionBuilderProvider { get; }
        public IServiceConnection Current { get; private set; }

        public void Dispose()
        {
            _resilientStream.OnCompleted();
            _activitySubject.OnCompleted();
            if (null != _resilientStreamProcess) _resilientStreamProcess.Dispose();
        }

        private void SetServiceActivity(bool isActive)
        {
            _isServiceActive = isActive;
            _activitySubject.OnNext(_isServiceActive);
        }


        public SignalRServiceClientBase(TRequest request, HttpTransportType transports, Action<HttpConnectionOptions> configureHttpConnection)
        {
            Request = request;
            TransportType = transports;
            HttpConnectionOptions = configureHttpConnection;

            InitializeInternal();
        }

        protected virtual void Initialize()
        {
        }

        private void InitializeInternal()
        {

            _activitySubject = new Subject<bool>();

            _onError = () => SetServiceActivity(false);

            _onSuccess = () => SetServiceActivity(true);

            IsActive = _activitySubject.AsObservable();

            Initialize();
        }

        public void BuildInternal()
        {
            var config = AppCore.Instance.Get<IHubConfiguration>();

            var hubConfig = config.Hubs.FirstOrDefault(hub => hub.Name == HubName);

            if (null == hubConfig) throw new MissingHubConfigException(HubName);

            ConnectionProvider = new ConnectionProvider(hubConfig, Request, TransportType, HttpConnectionOptions, ConnectionBuilderProvider);
        }

        public IObservable<TDto> Connect(IScheduler scheduler, long connectionTimeoutDelay)
        {

            if (!_isServiceActive)
            {
                _resilientStream = new Subject<TDto>();
                _doCancel = new CancellationTokenSource();
                _cancel = _doCancel.Token;
                _resilientStreamProcess = scheduler.Schedule(async () => await StartResilientStream(TimeSpan.FromMilliseconds(connectionTimeoutDelay)));
            }

            SetServiceActivity(true);

            return _resilientStream.AsObservable();
        }

        public void Disconnect()
        {
            //clear current stream(s)
            if (null != _resilientStreamProcess)
            {
                _resilientStream.OnCompleted();
                _resilientStreamProcess.Dispose();
            }

            //cancel resilience process
            _doCancel.Cancel();

            //stop connection
            Current.Stop();

            SetServiceActivity(false);
        }

        private IDisposable GetStreamForConnection(Action<TDto> onNext)
        {
            if (Current.CurrentState == ConnectionStatus.Closed) return Disposable.Empty;

            return Current.Proxy.On<TDto>(OnStreamUpdateMethodName, change =>
            {
                onNext(change);
            });

        }

        private async Task StartResilientStream(TimeSpan connectionTimeout)
        {
            if (_cancel.IsCancellationRequested) return;

            Current = ConnectionProvider.GetNextConnection(_onError, _onSuccess, Current == null ? null : Current.Endpoint);

            var isConnectionSet = await Current.Initialize();

            if (!isConnectionSet)
            {
                //if the connection failed to reach endpoint, we delay and try again
                await Task.Delay(connectionTimeout);
                await StartResilientStream(connectionTimeout);
            }
            else
            {
                Current.StatusStream.Subscribe(async current =>
                {
                    //if the connection abrutly closed, then we immediatly try to reach the next endpoint
                    if (current.ConnectionStatus == ConnectionStatus.Closed)
                    {
                        await StartResilientStream(connectionTimeout);
                    }
                });

                //if we conect to the current endpoint, then wire up the stream
                GetResilientStream(connectionTimeout);
            }
        }

        private void GetResilientStream(TimeSpan connectionTimeout)
        {
            //no need to track the diposable - it is handled when signalr close the connection
            GetStreamForConnection((change) => _resilientStream.OnNext(change));
        }
    }
}
