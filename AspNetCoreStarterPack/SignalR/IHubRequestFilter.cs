using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspNetCoreStarterPack.SignalR
{
    public interface IHubRequestFilter
    {
        Expression FilterExpression { get; }
        String GroupId { get; }
    }
}
