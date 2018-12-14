using Orc.DependencyGraph;

namespace Dasein.Core.Lite.Event
{
    public interface IEventProcessorTrace<TTarget, TContext,TEvent>
        where TEvent : IEvent
    {
        void Visit(INode<IIndicator<TTarget, TContext, TEvent>> node, TEvent ev, int notifiableCount);
    }
}