﻿using System;
using System.Reactive;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Dasein.Core.Lite.Shared
{
    public interface IServiceConnection
    {
        ConnectionStatus CurrentState { get; }
        IObservable<ConnectionInfo> StatusStream { get; }
        Task<bool> Initialize();
        Task Stop();
        string Endpoint { get; }
        HubConnection Proxy { get; }
    }
 }