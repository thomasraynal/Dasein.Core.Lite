using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace Dasein.Core.Lite.Shared
{
    public interface IConnectionProvider
    {
        IServiceConnection GetNextConnection(Action onError, Action onSuccess, String endpoint = null);
    }
}