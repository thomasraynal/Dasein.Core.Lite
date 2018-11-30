using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public static class ArrayExtensions
    {
        public static void Shuffle<T>(this IEnumerable<T> source)
        {
            var array = source.ToArray();
            var random = new Random(unchecked(Environment.TickCount * 31));
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }
    }
}
