﻿using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using AspNetCoreStarterPack.SignalR;
using AspNetCoreStarterPack.Infrastructure;

namespace AspNetCoreStarterPack.Default
{
    public abstract class StreamingServiceClientBase<TDto, TRequest> : ISignalRService<TDto, TRequest>
              where TRequest : IHubRequest<TDto>
    {
        private ISubject<TDto> _resilientStream;
        private ISubject<bool> _activitySubject;
        private CancellationTokenSource _doCancel;
        private bool _isServiceActive;
        private CancellationToken _cancel;
        private IDisposable _resilientStreamProcess;
        private readonly Action _onError;
        private readonly Action _onSuccess;

        public abstract String HubName { get; }
        public abstract String OnStreamUpdateMethodName { get; }
        public TRequest Request { get; }
        public IObservable<bool> IsActive { get; }
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

        public StreamingServiceClientBase(TRequest request)
        {
            Request = request;

            _activitySubject = new Subject<bool>();

            _onError = () => SetServiceActivity(false);

            _onSuccess = () => SetServiceActivity(true);

            IsActive = _activitySubject.AsObservable();

        }

        public void Initialize()
        {
            var config = AppCore.Instance.Get<IHubConfiguration>();

            var hubConfig = config.Hubs.FirstOrDefault(hub => hub.Name == HubName);

            if (null == hubConfig) throw new MissingHubConfigException(HubName);

            ConnectionProvider = new ConnectionProvider(hubConfig, Request, ConnectionBuilderProvider);
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
