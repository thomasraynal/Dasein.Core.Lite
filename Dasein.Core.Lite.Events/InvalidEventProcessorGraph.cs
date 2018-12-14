using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Event
{
    [Serializable]
    internal class InvalidEventProcessorGraph : Exception
    {
     
        public InvalidEventProcessorGraph() : base("Submitted sequence make the event processor graph invalid")
        {
        }

        protected InvalidEventProcessorGraph(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
