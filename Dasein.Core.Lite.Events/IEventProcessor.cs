using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Event
{
    public interface IEventProcessor<TTarget, TContext, TEvent>
               where TEvent : IEvent
    {
        Task Initialize();
        void AddRelation(IEnumerable<IIndicator<TTarget, TContext, TEvent>> relation);
        void AddRelations(IEnumerable<IEnumerable<IIndicator<TTarget, TContext, TEvent>>> relations);
        bool CanExecute(TEvent ev, TContext context);
        IEnumerable<TTarget> Execute(TEvent ev, TContext context, IEventProcessorTrace<TTarget, TContext, TEvent> trace);
    }
}