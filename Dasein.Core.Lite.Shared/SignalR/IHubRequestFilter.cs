using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public interface IHubRequestFilter
    {
        Expression FilterExpression { get; }
        String GroupId { get; }
    }
}
