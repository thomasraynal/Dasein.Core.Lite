using DynamicData;
using NUnit.Framework;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Dasein.Core.Lite.Event;
using System.Linq;

namespace Dasein.Core.Lite.Tests.Events
{
    [TestFixture]
    public class TestEvents
    {
        [Test]
        public async Task ShouldRunEvents()
        {
            var cache = new SourceCache<ItemEvent, Guid>(ev => ev.EventId);
            var eventProcessor = new ItemEventProcessor();

            await eventProcessor.Initialize();

            var change = "yadadadada";

            var now = DateTime.Now;

            var items = Enumerable.Range(0, 5).Select(_ => new Item() { Id = Guid.NewGuid(), LastChange = now, Tag = string.Empty }).ToList();
            var impactedItem = items.First();

            var disposable = cache
                                .Connect()
                                .ObserveOn(Scheduler.CurrentThread)
                                .ApplyEvent(eventProcessor, items)
                                .DisposeMany()
                                .Subscribe();


            cache.AddOrUpdate(new ItemEvent()
            {
                EventId = Guid.NewGuid(),
                ItemId = items.First().Id,
                Tag = change
            });

            Assert.AreEqual(impactedItem.Tag, change);

            Assert.IsTrue(impactedItem.LastChange > now);

            disposable.Dispose();
        }
    }
}
