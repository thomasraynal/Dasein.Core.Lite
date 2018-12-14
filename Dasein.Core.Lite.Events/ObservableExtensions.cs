using DynamicData;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Dasein.Core.Lite.Event
{
    public static class ObservableExtensions
    {
        public static IObservable<IChangeSet<TObject, TKey>> RunOnDispatcher<TObject, TKey>(this IObservable<IChangeSet<TObject, TKey>> source, Action<TObject> onNext, IScheduler scheduler)
        {

            return source.Do(changes =>
            {
                foreach (var change in changes)
                {
                    scheduler.Schedule(() => onNext(change.Current));

                }
            });
        }


        public static IObservable<IChangeSet<TEvent, TKey>> RaiseEvent<TEvent, TKey, TTarget, TContext>(
            this IObservable<IChangeSet<TEvent, TKey>> source,
            IEventProcessor<TTarget, TContext, TEvent> eventProcessor,
            TContext context,
            IEventProcessorTrace<TTarget, TContext, TEvent> trace = null
            )
          where TEvent : IEvent
        {

            return source
                .Filter(change => eventProcessor.CanExecute(change, context))
                .Do(changes =>
                {
                    foreach (var change in changes)
                    {
                        eventProcessor.Execute(change.Current, context, trace);
                    }

                });
        }

        public static IObservable<IEnumerable<TTarget>> ApplyEvent<TEvent, TTarget, TContext>(
            this IObservable<TEvent> source,
            IEventProcessor<TTarget, TContext, TEvent> eventProcessor,
            TContext context,
            IEventProcessorTrace<TTarget, TContext, TEvent> trace = null
            )
          where TEvent : IEvent
        {
           return source.Where(change => eventProcessor.CanExecute(change, context))
            .Select(change =>
            {

                return eventProcessor.Execute(change, context, trace);
            });
        }

        public static IObservable<IChangeSet<IEnumerable<TTarget>, TKey>> ApplyEvent<TEvent, TKey, TTarget, TContext>(
            this IObservable<IChangeSet<TEvent, TKey>> source,
            IEventProcessor<TTarget, TContext, TEvent> eventProcessor,
            TContext context,
            IEventProcessorTrace<TTarget, TContext, TEvent> trace = null
            )
          where TEvent : IEvent
        {

            return source
                .Filter(change => eventProcessor.CanExecute(change, context))
                .Transform(change =>
                {

                    return eventProcessor.Execute(change, context, trace);
                });
        }
    }
}
