using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Event
{
    public interface IIndicator<TTarget, TContext, TEvent> : IEquatable<IIndicator<TTarget, TContext, TEvent>>
        where TEvent: IEvent
    {
        String Label { get; }
        bool Accept(TEvent ev);
        Task Update(TEvent ev, TTarget target, TContext context);
    }
}
