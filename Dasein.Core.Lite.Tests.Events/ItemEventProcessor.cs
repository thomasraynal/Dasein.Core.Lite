using Dasein.Core.Lite.Event;
using Orc.DependencyGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Tests.Events
{
    public class TagIndicator : IndicatorBase<Item, IEnumerable<Item>, ItemEvent>
    {
        public TagIndicator() : base("Tag")
        {
        }

        public override bool Accept(ItemEvent ev)
        {
            return true;
        }

        public override Task Update(ItemEvent ev, Item item, IEnumerable<Item> context)
        {
            item.Tag = ev.Tag;
            return Task.CompletedTask;
        }
    }

    public class ChangeIndicator : IndicatorBase<Item, IEnumerable<Item>, ItemEvent>
    {
        public ChangeIndicator() : base("Change")
        {
        }

        public override bool Accept(ItemEvent ev)
        {
            return true;
        }

        public override Task Update(ItemEvent ev, Item item, IEnumerable<Item> context)
        {
            item.LastChange = DateTime.Now;
            return Task.CompletedTask;
        }
    }

    public class ItemEventProcessor : EventProcessor<Item, IEnumerable<Item>, ItemEvent>
    {

        protected override IEnumerable<Item> GetNotifiables(ItemEvent ev, IEnumerable<Item> context)
        {
            return context.Where(item => item.Id.ToString() == ev.Subject);
        }

        protected override Task InitializeInternal()
        {
            var tagIndicator = new TagIndicator();
            var changeIndicator = new ChangeIndicator();

            this.AddRelations(
                 new[]
                {
                   new List<IIndicator<Item, IEnumerable<Item>,ItemEvent>> { tagIndicator, changeIndicator },
                 }
               );

            return Task.CompletedTask;
        }

        protected override bool CanExcuteInternal(ItemEvent ev, IEnumerable<Item> context)
        {
            return GetNotifiables(ev, context).Any();
        }

        protected override void ProcessNode(INode<IIndicator<Item, IEnumerable<Item>, ItemEvent>> node, ItemEvent ev, IEnumerable<Item> notifyables, IEnumerable<Item> context)
        {
            foreach (var pose in notifyables)
            {
                node.Value.Update(ev, pose, context);
            }
        }
    }
}
