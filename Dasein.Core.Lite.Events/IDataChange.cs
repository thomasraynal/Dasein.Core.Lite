using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Event
{
    public interface IDataChange<TData>
    {
        DataChangeReason Reason { get; }
        TData Data { get; }
    }
}
