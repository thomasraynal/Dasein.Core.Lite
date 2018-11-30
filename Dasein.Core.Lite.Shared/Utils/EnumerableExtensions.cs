using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Dasein.Core.Lite.Shared
{
    public static class EnumerableExtensions
    {
        private static Random _rand;

        static EnumerableExtensions()
        {
            _rand = new Random();
        }

        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source)
        {
            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    for (var value = e.Current; e.MoveNext(); value = e.Current)
                    {
                        yield return value;
                    }
                }
            }
        }

        public static IEnumerable<T> YieldOne<T>(this T source)
        {
            yield return source;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int i = 0;
            foreach (var item in source)
            {
                action(item, i);
                i++;
            }
        }
        public static IEnumerable<T> FromDelimited<T>(this string source, Func<string, T> converter, string delimiter = ",")
        {
            if (string.IsNullOrWhiteSpace(source))
                yield break;

            var strings = source.Split(delimiter.ToCharArray());
            if (!strings.Any()) yield break;

            foreach (var s in strings)
                yield return converter(s);
        }

        public static string ToDelimited<T>(this IEnumerable<T> source, string delimiter = ",")
        {
            var array = source.ToArray();
            return !array.Any() ? string.Empty : string.Join(string.Empty, array.WithDelimiter(delimiter));
        }

        public static IEnumerable<string> WithDelimiter<T>(this IEnumerable<T> source, string delimiter)
        {
            var array = source.ToArray();
            if (!array.Any()) yield return string.Empty;

            yield return array.Select(t => t.ToString()).First();

            foreach (var item in array.Skip(1))
                yield return $"{delimiter}{item}";

        }

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            var list = enumerable as IList<T> ?? enumerable.ToList();
            return list.Count == 0 ? default(T) : list[_rand.Next(0, list.Count)];
        }
    }
}
