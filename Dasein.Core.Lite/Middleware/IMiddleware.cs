using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite
{
    public interface IMiddleware<TService>
    {
         TService Service { get; }
    }
}
