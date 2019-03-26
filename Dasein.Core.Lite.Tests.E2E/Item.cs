using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Tests.Events
{
    public class Item
    {
        public Guid Id { get; set; }
        public String Tag { get; set; }
        public DateTime LastChange { get; set; }
    }
}
