using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace ConsoleApp.Helpers
{
    public static class DeconstructHelpers
    {
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }

        public static void Deconstruct<TKey, TElement>([NotNull] this IGrouping<TKey, TElement> group, out TKey key, [NotNull] out IEnumerable<TElement> elements)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            key = group.Key;
            elements = group;
        }
    }
}