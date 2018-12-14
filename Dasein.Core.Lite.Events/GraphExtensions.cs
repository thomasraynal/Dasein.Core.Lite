using Orc.DependencyGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Event
{
    public static class GraphExtensions
    {
        public static IOrderedEnumerable<INode<T>> UniqueDescendants<T>(this INode<T> node)
            where T : IEquatable<T>
        {
            return node.Descendants.Distinct().OrderBy(x => x.Level);
        }


    }
}
