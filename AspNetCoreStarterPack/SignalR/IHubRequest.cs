using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.SignalR
{
    public interface IHubRequest<TDto>
    {
        //IDictionary<string, string>
        String GroupId { get; }
        bool Accept(TDto dto);
    }
}
