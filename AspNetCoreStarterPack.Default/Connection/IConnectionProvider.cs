using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace AspNetCoreStarterPack.Default
{
    public interface IConnectionProvider
    {
        IServiceConnection GetNextConnection(Action onError, Action onSuccess, String endpoint = null);
    }
}