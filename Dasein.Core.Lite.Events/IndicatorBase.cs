using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Event
{
    public abstract class IndicatorBase<TTarget, TContext, TEvent> : IIndicator<TTarget, TContext, TEvent>
               where TEvent : IEvent
    {
        public String Label { get; private set; }

        public IndicatorBase(String label)
        {
            Label = label;
        }

        public abstract bool Accept(TEvent ev);
        public abstract Task Update(TEvent ev, TTarget position, TContext context);

        public bool Equals(IIndicator<TTarget, TContext, TEvent> other)
        {
            return other.Label == Label;
        }


    }
}
