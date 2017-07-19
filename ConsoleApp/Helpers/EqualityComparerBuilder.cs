using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace ConsoleApp.Helpers
{
    internal static class EqualityComparerBuilder
    {
        private sealed class Comparer<T> : IEqualityComparer<T>
        {
            private readonly IReadOnlyCollection<Func<T, T, bool>> _equals;
            private readonly IReadOnlyCollection<Func<T, int>> _hashCodes;

            public Comparer(IReadOnlyCollection<Func<T, T, bool>> equals, IReadOnlyCollection<Func<T, int>> hashCodes)
            {
                _equals = equals;
                _hashCodes = hashCodes;
            }

            public bool Equals([NotNull] T x, [NotNull] T y)
            {
                if (x == null)
                    throw new ArgumentNullException(nameof(x));
                if (y == null)
                    throw new ArgumentNullException(nameof(y));

                return _equals.All(eq => eq(x, y));
            }

            public int GetHashCode([NotNull] T obj)
            {
                if (obj == null)
                    throw new ArgumentNullException(nameof(obj));

                return _hashCodes.Aggregate(397, (acc, memberHash) => acc ^ memberHash(obj));
            }
        }

        public static IEqualityComparer<T> Build<T>([NotNull] params (Func<T, T, bool> memberEquals, Func<T, int> memberHashCode)[] funcs)
        {
            if (funcs == null)
                throw new ArgumentNullException(nameof(funcs));
            if (funcs.Length == 0)
                throw new ArgumentException($"{nameof(funcs)} cannot be an empty collection.", nameof(funcs));
            if (funcs.Any(pair => pair.memberEquals == null || pair.memberHashCode == null))
                throw new ArgumentException($"{nameof(funcs)} contains null func.", nameof(funcs));

            return new Comparer<T>(
                funcs.Select(pair => pair.memberEquals).ToList(),
                funcs.Select(pair => pair.memberHashCode).ToList());
        }
    }
}