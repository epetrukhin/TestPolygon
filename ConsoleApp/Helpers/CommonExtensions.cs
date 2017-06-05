using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using Functional;
using JetBrains.Annotations;

namespace ConsoleApp.Helpers
{
    [PublicAPI]
    internal static class CommonExtensions
    {
        #region Tuples
        private static readonly Type[] TupleTypes =
        {
            typeof(Tuple<>),
            typeof(Tuple<,>),
            typeof(Tuple<,,>),
            typeof(Tuple<,,,>),
            typeof(Tuple<,,,,>),
            typeof(Tuple<,,,,,>),
            typeof(Tuple<,,,,,,>),
            typeof(Tuple<,,,,,,,>)
        };

        [NotNull, UsedImplicitly]
        private static string ConvertTuple8ToString<T1, T2, T3, T4, T5, T6, T7, TRest>([NotNull] Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
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
        private static string ConvertTuple7ToString<T1, T2, T3, T4, T5, T6, T7>([NotNull] Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
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
        private static string ConvertTuple6ToString<T1, T2, T3, T4, T5, T6>([NotNull] Tuple<T1, T2, T3, T4, T5, T6> tuple)
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
        private static string ConvertTuple5ToString<T1, T2, T3, T4, T5>([NotNull] Tuple<T1, T2, T3, T4, T5> tuple)
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
        private static string ConvertTuple4ToString<T1, T2, T3, T4>([NotNull] Tuple<T1, T2, T3, T4> tuple)
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
        private static string ConvertTuple3ToString<T1, T2, T3>([NotNull] Tuple<T1, T2, T3> tuple)
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
        private static string ConvertTuple2ToString<T1, T2>([NotNull] Tuple<T1, T2> tuple)
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
        private static string ConvertTuple1ToString<T1>([NotNull] Tuple<T1> tuple)
        {
            Debug.Assert(tuple != null);

            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(")")
                .ToString();
        }
        #endregion

        #region Value Tuples
        private static readonly Type[] ValueTupleTypes =
        {
            typeof(ValueTuple<>),
            typeof(ValueTuple<,>),
            typeof(ValueTuple<,,>),
            typeof(ValueTuple<,,,>),
            typeof(ValueTuple<,,,,>),
            typeof(ValueTuple<,,,,,>),
            typeof(ValueTuple<,,,,,,>),
            typeof(ValueTuple<,,,,,,,>)
        };

        [NotNull, UsedImplicitly]
        private static string ConvertValueTuple8ToString<T1, T2, T3, T4, T5, T6, T7, TRest>(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
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
        private static string ConvertValueTuple7ToString<T1, T2, T3, T4, T5, T6, T7>(ValueTuple<T1, T2, T3, T4, T5, T6, T7> tuple)
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
        private static string ConvertValueTuple6ToString<T1, T2, T3, T4, T5, T6>(ValueTuple<T1, T2, T3, T4, T5, T6> tuple)
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
        private static string ConvertValueTuple5ToString<T1, T2, T3, T4, T5>(ValueTuple<T1, T2, T3, T4, T5> tuple)
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
        private static string ConvertValueTuple4ToString<T1, T2, T3, T4>(ValueTuple<T1, T2, T3, T4> tuple)
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
        private static string ConvertValueTuple3ToString<T1, T2, T3>(ValueTuple<T1, T2, T3> tuple)
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
        private static string ConvertValueTuple2ToString<T1, T2>(ValueTuple<T1, T2> tuple)
        {
            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(", ")
                .Append(tuple.Item2.ConvertToString())
                .Append(")")
                .ToString();
        }

        [NotNull, UsedImplicitly]
        private static string ConvertValueTuple1ToString<T1>(ValueTuple<T1> tuple)
        {
            return new StringBuilder("(")
                .Append(tuple.Item1.ConvertToString())
                .Append(")")
                .ToString();
        }
        #endregion

        [NotNull, UsedImplicitly]
        private static string ConvertIEnumerableToString<T>([NotNull] IEnumerable<T> sequence)
        {
            Debug.Assert(sequence != null);

            return "[" + string.Join(", ", sequence.Select(item => item.ConvertToString())) + "]";
        }

        [NotNull, UsedImplicitly]
        private static string ConvertIGroupingToString<TKey, TElement>([NotNull] IGrouping<TKey, TElement> group)
        {
            Debug.Assert(group != null);

            return group.Key.ConvertToString() + " => [" + string.Join(", ", group.Select(item => item.ConvertToString())) + "]";
        }

        [NotNull, UsedImplicitly]
        private static string ConvertIDictionaryToString<TKey, TValue>([NotNull] IDictionary<TKey, TValue> dictionary)
        {
            Debug.Assert(dictionary != null);

            return "[" + string.Join(", ", dictionary.Select(kvp => kvp.Key.ConvertToString() + " => " + kvp.Value.ConvertToString())) + "]";
        }

        [NotNull, UsedImplicitly]
        private static string ConvertMaybeToString<T>(Maybe<T> maybe) => maybe.HasValue
            ? $"Some({maybe.Value.ConvertToString()})"
            : "None";

        [NotNull, UsedImplicitly]
        private static string ConvertEitherToString<TLeft, TRight>([NotNull] Either<TLeft, TRight> either)
        {
            Debug.Assert(either != null);

            return either.Case(
                left => $"Left({left.ConvertToString()})",
                right => $"Right({right.ConvertToString()})");
        }

        [CanBeNull]
        private static Type[] TryGetGenericTypeArgumentsForGenericInterface([NotNull] Type type, [NotNull] Type genericInterfaceType)
        {
            Debug.Assert(type != null);
            Debug.Assert(genericInterfaceType != null);
            Debug.Assert(genericInterfaceType.IsGenericTypeDefinition);


            return type.GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericInterfaceType)
                .Select(t => t.GenericTypeArguments)
                .FirstOrDefault();
        }

        [NotNull]
        private static string CallGenericMethod([NotNull] object obj, [NotNull] string methodName, [NotNull] Type[] genericTypeArgs)
        {
            Debug.Assert(obj != null);
            Debug.Assert(!string.IsNullOrWhiteSpace(methodName));
            Debug.Assert(genericTypeArgs != null);

            try
            {
                return typeof(CommonExtensions)
                    .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static)
                    .MakeGenericMethod(genericTypeArgs)
                    .Invoke(null, new[] { obj })
                    .ToString();
            }
            catch (TargetInvocationException e)
            {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                // ReSharper disable once HeuristicUnreachableCode
                throw;
            }
        }

        [NotNull, PublicAPI]
        public static string ConvertToString([CanBeNull] this object obj)
        {
            if (obj == null)
                return "<null>";

            if (obj is string)
                return "\"" + obj + "\"";

            if (obj is char)
                return "'" + obj + "'";

            var type = obj.GetType();

            if (obj is Exception ex)
                return type.Name + "(" + ex.Message + ")";

            var dictionaryGenericArgs = TryGetGenericTypeArgumentsForGenericInterface(type, typeof(IDictionary<,>));
            if (dictionaryGenericArgs != null)
                return CallGenericMethod(obj, nameof(ConvertIDictionaryToString), dictionaryGenericArgs);

            var groupingGenericArgs = TryGetGenericTypeArgumentsForGenericInterface(type, typeof(IGrouping<,>));
            if (groupingGenericArgs != null)
                return CallGenericMethod(obj, nameof(ConvertIGroupingToString), groupingGenericArgs);

            var enumerableGenericArg = TryGetGenericTypeArgumentsForGenericInterface(type, typeof(IEnumerable<>));
            if (enumerableGenericArg != null)
                return CallGenericMethod(obj, nameof(ConvertIEnumerableToString), enumerableGenericArg);

            if (type.IsGenericType)
            {
                if (TupleTypes.Contains(type.GetGenericTypeDefinition()))
                    return CallGenericMethod(obj, $"ConvertTuple{type.GenericTypeArguments.Length}ToString", type.GenericTypeArguments);

                if (ValueTupleTypes.Contains(type.GetGenericTypeDefinition()))
                    return CallGenericMethod(obj, $"ConvertValueTuple{type.GenericTypeArguments.Length}ToString", type.GenericTypeArguments);

                if (type.GetGenericTypeDefinition() == typeof(Maybe<>))
                    return CallGenericMethod(obj, nameof(ConvertMaybeToString), type.GenericTypeArguments);
            }

            if (type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(Either<,>))
                return CallGenericMethod(obj, nameof(ConvertEitherToString), type.BaseType.GenericTypeArguments);

            if (typeof(IEnumerable).IsAssignableFrom(type))
                return ConvertIEnumerableToString(((IEnumerable)obj).Cast<object>());

            return obj.ToString();
        }

        public static void Dump<T>([CanBeNull] this T obj, [CanBeNull] object name = null)
        {
            var nameString = name?.ToString();

            Console.WriteLine(string.IsNullOrWhiteSpace(nameString) ? obj.ConvertToString() : nameString + ": " + obj.ConvertToString());
        }

        public static void DumpObservable<T>([CanBeNull] this IObservable<T> observable, [CanBeNull] object name = null)
        {
            if (observable == null)
            {
                Dump<object>(null, name);
                return;
            }

            observable
                .Trace(name)
                .Subscribe();
        }

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

            void Action(string str) => Console.WriteLine(string.IsNullOrWhiteSpace(nameString) ? str : nameString + ": " + str);

            return source.Do(
                item => Action("OnNext(" + item.ConvertToString() + ")"),
                exception => Action("OnError(" + exception.ConvertToString() + ")"),
                () => Action("OnCompleted"));
        }

        public static void WriteLine([CanBeNull] this string text) => Console.WriteLine(text ?? "<null>");

        public static void WriteError([CanBeNull] this string text)
        {
            using (ConsoleHelpers.WithForegroundColor(ConsoleColor.Red))
            {
                text.WriteLine();
            }
        }

        public static void WriteWarning([CanBeNull] this string text)
        {
            using (ConsoleHelpers.WithForegroundColor(ConsoleColor.Yellow))
            {
                text.WriteLine();
            }
        }

        public static void WriteInfo([CanBeNull] this string text)
        {
            using (ConsoleHelpers.WithForegroundColor(ConsoleColor.Blue))
            {
                text.WriteLine();
            }
        }
    }
}