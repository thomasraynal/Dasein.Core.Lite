using Dasein.Core.Lite.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Tests.Events
{
    public class ItemEvent : IEvent
    {
        public Guid ItemId { get; set; }
        public Guid EventId { get; set; }
        public String Tag { get; set; }

        public string Name => EventId.ToString();

        public string Subject => ItemId.ToString();
    }
}
