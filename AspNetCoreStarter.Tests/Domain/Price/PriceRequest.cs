using AspNetCoreStarterPack.SignalR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspNetCoreStarter.Tests.Domain
{
    public class PriceRequest : HubRequestBase<Price>
    {
        public PriceRequest(Expression<Func<Price, bool>> filter) : base(filter)
        {
        }
    }
}
