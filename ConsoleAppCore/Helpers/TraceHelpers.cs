using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using JetBrains.Annotations;

namespace ConsoleAppCore.Helpers
{
    [PublicAPI]
    internal static class TraceHelpers
    {
        [NotNull]
        public static IEnumerable<T> Trace<T>([NotNull] this IEnumerable<T> source, [CanBeNull] object name = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            List<T> result;

            try
            {
                result = source.ToList();
                result.Dump(name);
            }
            catch (Exception e)
            {
                e.Dump(name);
                throw;
            }

            return result;
        }

        [NotNull]
        public static IObservable<T> Trace<T>([NotNull] this IObservable<T> source, [CanBeNull] object name = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var nameString = name?.ToString();

            void Action(string str) =>
                (string.IsNullOrWhiteSpace(nameString) ? str : $"{nameString}: {str}").WriteLine();

            return source.Do(
                item      => Action($"OnNext({item.ConvertToString()})"),
                exception => Action($"OnError({exception.ConvertToString()})"),
                ()        => Action("OnCompleted"));
        }
    }
}