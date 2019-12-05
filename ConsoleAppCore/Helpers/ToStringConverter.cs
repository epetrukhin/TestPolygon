using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Functional;
using JetBrains.Annotations;

namespace ConsoleAppCore.Helpers
{
    internal static class ToStringConverter
    {
        #region Tuples
        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2, T3, T4, T5, T6, T7, TRest>([NotNull] Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
        {
            Debug.Assert(tuple != null);

            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(", ")
                .Append(tuple.Item3.ConvertToString())
                .Append(", ")
                .Append(tuple.Item4.ConvertToString())
                .Append(", ")
                .Append(tuple.Item5.ConvertToString())
                .Append(", ")
                .Append(tuple.Item6.ConvertToString())
                .Append(", ")
                .Append(tuple.Item7.ConvertToString())
                .Append(", ")
                .Append(tuple.Rest.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2, T3, T4, T5, T6, T7>([NotNull] Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            Debug.Assert(tuple != null);

            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(", ")
                .Append(tuple.Item3.ConvertToString())
                .Append(", ")
                .Append(tuple.Item4.ConvertToString())
                .Append(", ")
                .Append(tuple.Item5.ConvertToString())
                .Append(", ")
                .Append(tuple.Item6.ConvertToString())
                .Append(", ")
                .Append(tuple.Item7.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2, T3, T4, T5, T6>([NotNull] Tuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            Debug.Assert(tuple != null);

            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(", ")
                .Append(tuple.Item3.ConvertToString())
                .Append(", ")
                .Append(tuple.Item4.ConvertToString())
                .Append(", ")
                .Append(tuple.Item5.ConvertToString())
                .Append(", ")
                .Append(tuple.Item6.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2, T3, T4, T5>([NotNull] Tuple<T1, T2, T3, T4, T5> tuple)
        {
            Debug.Assert(tuple != null);

            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(", ")
                .Append(tuple.Item3.ConvertToString())
                .Append(", ")
                .Append(tuple.Item4.ConvertToString())
                .Append(", ")
                .Append(tuple.Item5.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2, T3, T4>([NotNull] Tuple<T1, T2, T3, T4> tuple)
        {
            Debug.Assert(tuple != null);

            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(", ")
                .Append(tuple.Item3.ConvertToString())
                .Append(", ")
                .Append(tuple.Item4.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2, T3>([NotNull] Tuple<T1, T2, T3> tuple)
        {
            Debug.Assert(tuple != null);

            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(", ")
                .Append(tuple.Item3.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2>([NotNull] Tuple<T1, T2> tuple)
        {
            Debug.Assert(tuple != null);

            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1>([NotNull] Tuple<T1> tuple)
        {
            Debug.Assert(tuple != null);

            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(")")
                .ToString();
        }
        #endregion

        #region Value Tuples
        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2, T3, T4, T5, T6, T7, TRest>(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
            where TRest : struct
        {
            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(", ")
                .Append(tuple.Item3.ConvertToString())
                .Append(", ")
                .Append(tuple.Item4.ConvertToString())
                .Append(", ")
                .Append(tuple.Item5.ConvertToString())
                .Append(", ")
                .Append(tuple.Item6.ConvertToString())
                .Append(", ")
                .Append(tuple.Item7.ConvertToString())
                .Append(", ")
                .Append(tuple.Rest.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2, T3, T4, T5, T6, T7>(ValueTuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(", ")
                .Append(tuple.Item3.ConvertToString())
                .Append(", ")
                .Append(tuple.Item4.ConvertToString())
                .Append(", ")
                .Append(tuple.Item5.ConvertToString())
                .Append(", ")
                .Append(tuple.Item6.ConvertToString())
                .Append(", ")
                .Append(tuple.Item7.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2, T3, T4, T5, T6>(ValueTuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(", ")
                .Append(tuple.Item3.ConvertToString())
                .Append(", ")
                .Append(tuple.Item4.ConvertToString())
                .Append(", ")
                .Append(tuple.Item5.ConvertToString())
                .Append(", ")
                .Append(tuple.Item6.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2, T3, T4, T5>(ValueTuple<T1, T2, T3, T4, T5> tuple)
        {
            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(", ")
                .Append(tuple.Item3.ConvertToString())
                .Append(", ")
                .Append(tuple.Item4.ConvertToString())
                .Append(", ")
                .Append(tuple.Item5.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2, T3, T4>(ValueTuple<T1, T2, T3, T4> tuple)
        {
            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(", ")
                .Append(tuple.Item3.ConvertToString())
                .Append(", ")
                .Append(tuple.Item4.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2, T3>(ValueTuple<T1, T2, T3> tuple)
        {
            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(", ")
                .Append(tuple.Item3.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1, T2>(ValueTuple<T1, T2> tuple)
        {
            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string Convert<T1>(ValueTuple<T1> tuple)
        {
            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(")")
                .ToString();
        }
        #endregion

        #region Concrete types to string
        [NotNull]
        private static string Convert<T>([NotNull] IEnumerable<T> sequence) =>
            "[" + string.Join(", ", sequence.Select(item => item.ConvertToString())) + "]";

        [NotNull]
        private static string Convert<TKey, TElement>([NotNull] IGrouping<TKey, TElement> group) =>
            group.Key.ConvertToString() + " => " + group.ToList().ConvertToString();

        [NotNull]
        private static string Convert<TKey, TValue>(KeyValuePair<TKey, TValue> kvp) =>
            kvp.Key.ConvertToString() + " => " + kvp.Value.ConvertToString();

        [NotNull]
        private static string Convert<T>(Maybe<T> maybe) =>
            maybe.HasValue
                ? $"Some({maybe.Value.ConvertToString()})"
                : "None";

        [NotNull]
        private static string Convert<TLeft, TRight>([NotNull] Either<TLeft, TRight> either) =>
            either.Case(
                left => $"Left({left.ConvertToString()})",
                right => $"Right({right.ConvertToString()})");

        [NotNull]
        private static string Convert([NotNull] string str) => $"\"{str}\"";

        [NotNull]
        private static string Convert(char c) => $"\'{c}\'";

        [NotNull]
        private static string Convert([NotNull] Exception ex) =>
            $"{ex.GetType().Name}({ex.Message})";

        [NotNull]
        private static string Convert([NotNull] object obj) =>
            obj.ToString();
        #endregion

        [NotNull, PublicAPI]
        public static string ConvertToString([CanBeNull] this object obj) =>
            obj == null
                ? "<null>"
                : (string)Convert((dynamic)obj);
    }
}