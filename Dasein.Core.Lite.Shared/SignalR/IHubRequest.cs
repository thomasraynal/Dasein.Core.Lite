using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public interface IHubRequest<TDto> : IHubRequestFilter
    {
       Func<TDto,bool> Filter { get; }
    }
}
