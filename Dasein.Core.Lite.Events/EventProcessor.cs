using Orc.DependencyGraph;
using Orc.DependencyGraph.GraphD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Event
{
    [Serializable]
    public class HandleEventException : Exception
    {
        public HandleEventException(String message, Exception innerException) : base(message, innerException)
        {
        }

        protected HandleEventException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public abstract class EventProcessor<TTarget, TContext, TEvent> : IEventProcessor<TTarget, TContext, TEvent>
        where TEvent : IEvent
    {
        protected Graph<IIndicator<TTarget, TContext, TEvent>> _graph;
        private IOrderedEnumerable<INode<IIndicator<TTarget, TContext, TEvent>>> _sortedGraph;

        public EventProcessor()
        {
            _graph = new Graph<IIndicator<TTarget, TContext, TEvent>>();
        }

        public void AddRelations(IEnumerable<IEnumerable<IIndicator<TTarget, TContext, TEvent>>> relations)
        {
            foreach(var sequence in relations)
            {
                if (!_graph.CanSort(sequence)) throw new InvalidEventProcessorGraph();
            }

            _graph.AddSequences(relations);
        }

        public void AddRelation(IEnumerable<IIndicator<TTarget, TContext, TEvent>> relation)
        {
            if (!_graph.CanSort(relation)) throw new InvalidEventProcessorGraph();
            _graph.AddSequence(relation);
        }

        protected virtual void ProcessNode(INode<IIndicator<TTarget, TContext, TEvent>> node, TEvent ev, IEnumerable<TTarget> notifyables, TContext context)
        {
            foreach (var notifyable in notifyables)
            {
                try
                {
                    //run synchrone
                    node.Value.Update(ev, notifyable, context).Wait();
                }
                catch (Exception ex)
                {
                    var messsage = $"Event Processor failed to handle event [{ev.Name}-{ev.Subject}] for node [{node.Value.Label}] (Entity : [{notifyable}])";
                    throw new HandleEventException(messsage, ex);
                }
            }
        }

        protected abstract IEnumerable<TTarget> GetNotifiables(TEvent ev, TContext context);

        public IEnumerable<TTarget> Execute(TEvent ev, TContext context, IEventProcessorTrace<TTarget, TContext, TEvent> trace)
        {
            var nodes = _sortedGraph.Where(n => n.Value.Accept(ev));

            var notifiables = GetNotifiables(ev, context);

            foreach (var node in nodes)
            {
                if (null != trace) trace.Visit(node, ev, notifiables.Count());

                ProcessNode(node, ev, notifiables, context);
            }

            return notifiables;
        }

        protected virtual Task InitializeInternal()
        {
            return Task.CompletedTask;
        }

        public async Task Initialize()
        {
            await InitializeInternal();
            _sortedGraph = _graph.Sort();
        }

        public bool CanExecute(TEvent ev, TContext context)
        {
            return _graph.CanSort() &&  CanExcuteInternal(ev, context);
        }

        protected virtual bool CanExcuteInternal(TEvent ev, TContext context)
        {
            return true;
        }
    }
}
