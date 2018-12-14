using Orc.DependencyGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Event
{
    public abstract class EventProcessorTrace<TTarget, TContext, TEvent> : IEventProcessorTrace<TTarget, TContext,TEvent>
           where TEvent : IEvent
    {
        public abstract void Visit(INode<IIndicator<TTarget, TContext, TEvent>> node, TEvent ev, int notifyableCount);
    }
}
