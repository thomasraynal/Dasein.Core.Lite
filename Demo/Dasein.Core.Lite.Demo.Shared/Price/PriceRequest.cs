using Dasein.Core.Lite.Shared;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Dasein.Core.Lite.Demo.Shared
{
    public class PriceRequest : HubRequestBase<Price>
    {
        public PriceRequest(Expression<Func<Price, bool>> filter) : base(filter)
        {
        }
    }
}
