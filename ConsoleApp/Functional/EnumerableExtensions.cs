using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Functional
{
    [PublicAPI]
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Индекс ненайденного значения.
        /// </summary>
        private const int NotFound = -1;

        /// <summary>
        /// Возвращает последовательность, образованную добавлением <paramref name="item"/> в
        /// конец последовательности <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, в конец которой добавляется элемент <paramref name="item"/>.</param>
        /// <param name="item">Добавляемый элемент.</param>
        /// <returns>
        /// Последовательность, образованная добавлением <paramref name="item"/> в
        /// конец последовательности <paramref name="sequence"/>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        [NotNull, Pure]
        public static IEnumerable<TSource> ContinueWith<TSource>([NotNull] this IEnumerable<TSource> sequence, TSource item)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            return sequence.Concat(item.AsSequence());
        }

        /// <summary>
        /// Возвращает последовательность, образованную добавлением <paramref name="items"/> в
        /// конец последовательности <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, в конец которой добавляется элементы <paramref name="items"/>.</param>
        /// <param name="items">Добавляемые элементы.</param>
        /// <returns>
        /// Последовательность, образованная добавлением <paramref name="items"/> в
        /// конец последовательности <paramref name="sequence"/>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c> или <paramref name="items"/><c> == null</c>.
        /// </exception>
        [NotNull, Pure]
        public static IEnumerable<TSource> ContinueWith<TSource>([NotNull] this IEnumerable<TSource> sequence, [NotNull] params TSource[] items)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            return sequence.Concat(items);
        }

        /// <summary>
        /// Возвращает последовательность, образованную вставкой <paramref name="item"/> в
        /// последовательность <paramref name="sequence"/> в позицию с индексом <paramref name="index"/>.
        /// Если количество элементов в последовательности <paramref name="sequence"/> больше или равно
        /// значению <paramref name="index"/>, <paramref name="item"/> добавляется в конец последовательности.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, в которую вставляется элемент <paramref name="item"/>.</param>
        /// <param name="index">
        /// Отсчитываемый от нуля индекс позиции в последовательности <paramref name="sequence"/>
        /// в которую будет вставлен элемент <paramref name="item"/>.
        /// </param>
        /// <param name="item">Вставляемый элемент.</param>
        /// <returns>
        /// Последовательность, образованная вставкой <paramref name="item"/> в
        /// последовательность <paramref name="sequence"/> в позицию с индексом <paramref name="index"/>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/><c> &lt; 0</c>.
        /// </exception>
        [NotNull, Pure]
        public static IEnumerable<TSource> SequenceInsert<TSource>([NotNull] this IEnumerable<TSource> sequence, int index, TSource item)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Index cannot be less than zero.");

            return SequenceInsertCore();

            IEnumerable<TSource> SequenceInsertCore()
            {
                var counter = 0;
                var inserted = false;
                foreach (var t in sequence)
                {
                    if (!inserted)
                    {
                        if (counter == index)
                        {
                            inserted = true;
                            yield return item;
                        }
                        else
                        {
                            counter++;
                        }
                    }
                    yield return t;
                }
                if (!inserted)
                    yield return item;
            }
        }

        /// <summary>
        /// Преобразует элемент <paramref name="item"/> в последовательность, состоящую из одного элемента.
        /// </summary>
        /// <typeparam name="TSource">Тип элемента <paramref name="item"/> и тип элементов возвращаемой последовательности.</typeparam>
        /// <param name="item">Элемент, преобразуемый в последовательность.</param>
        /// <returns>Последовательность, состоящая из одного элемента, <paramref name="item"/>.</returns>
        [NotNull, Pure]
        public static IEnumerable<TSource> AsSequence<TSource>(this TSource item) => EnumerableEx.Return(item);

        /// <summary>
        /// Проверяет, что последовательность <paramref name="sequence"/> содержит
        /// не менее чем <paramref name="count"/> элементов.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Проверяемая последовательность</param>
        /// <param name="count">Минимально допустимое количество элементов.</param>
        /// <returns>
        /// <c>true</c>, если количество элементов в последовательности <paramref name="sequence"/> больше или равно <paramref name="count"/>,
        /// <c>false</c> в противном случае.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/><c> &lt; 0.</c>
        /// </exception>
        [Pure]
        public static bool AtLeast<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence, int count)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count cannot be less than zero.");

            var counter = 0;
            using (var enumerable = sequence.GetEnumerator())
            {
                while (enumerable.MoveNext())
                {
                    if (++counter >= count)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Проверяет, что последовательность <paramref name="sequence"/> содержит
        /// не менее чем <paramref name="count"/> элементов, удовлетворяющих предикату <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Проверяемая последовательность</param>
        /// <param name="count">Минимально допустимое количество элементов.</param>
        /// <param name="predicate">Предикат, определяющий учитываемые элементы.</param>
        /// <returns>
        /// <c>true</c>, если количество элементов в последовательности <paramref name="sequence"/>, удовлетворяющих предикату,
        /// больше или равно <paramref name="count"/>,
        /// <c>false</c> в противном случае.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/><c> &lt; 0.</c>
        /// </exception>
        [Pure]
        public static bool AtLeast<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence, int count,
            [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count cannot be less than zero.");

            return sequence.Where(predicate).AtLeast(count);
        }

        /// <summary>
        /// Проверяет, что последовательность <paramref name="sequence"/> содержит
        /// не более чем <paramref name="count"/> элементов.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Проверяемая последовательность</param>
        /// <param name="count">Максимально допустимое количество элементов.</param>
        /// <returns>
        /// <c>true</c>, если количество элементов в последовательности <paramref name="sequence"/> меньше или равно <paramref name="count"/>,
        /// <c>false</c> в противном случае.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/><c> &lt; 0.</c>
        /// </exception>
        [Pure]
        public static bool AtMost<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence, int count)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count cannot be less than zero.");

            var counter = 0;
            using (var enumerable = sequence.GetEnumerator())
            {
                while (enumerable.MoveNext())
                {
                    if (++counter > count)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Проверяет, что последовательность <paramref name="sequence"/> содержит
        /// не более чем <paramref name="count"/> элементов, удовлетворяющих предикату <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Проверяемая последовательность</param>
        /// <param name="count">Максимально допустимое количество элементов.</param>
        /// <param name="predicate">Предикат, определяющий учитываемые элементы.</param>
        /// <returns>
        /// <c>true</c>, если количество элементов в последовательности <paramref name="sequence"/>, удовлетворяющих предикату,
        /// меньше или равно <paramref name="count"/>,
        /// <c>false</c> в противном случае.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/><c> &lt; 0.</c>
        /// </exception>
        [Pure]
        public static bool AtMost<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence, int count,
            [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count cannot be less than zero.");

            return sequence.Where(predicate).AtMost(count);
        }

        /// <summary>
        /// Ищет дубликаты в последовательности <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, в которой ищутся дубликаты.</param>
        /// <returns>true, если дубликаты есть, иначе false.</returns>
        [Pure]
        public static bool HasDuplicates<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            var hashSet = new HashSet<TSource>();
            return sequence.Any(e => !hashSet.Add(e));
        }

        /// <summary>
        /// Получает положение элемента <paramref name="element"/> в последовательности <paramref name="sequence"/>.
        /// Равенство проверяется методом TSource.Equals. Если элемент встречается несколько раз,
        /// возвращается положение первого из них.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, по которой производится поиск.</param>
        /// <param name="element">Искомый элемент.</param>
        /// <returns>Положение элемента в последовательности, если он найден; <see cref="NotFound"/> (-1) в противном случае.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        [Pure]
        public static int IndexOf<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence, TSource element)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            var comparer = EqualityComparer<TSource>.Default;

            var index = 0;
            foreach (var item in sequence)
            {
                if (comparer.Equals(element, item))
                    return index;
                index++;
            }
            return NotFound;
        }

        /// <summary>
        /// Получает положение элемента в последовательности <paramref name="sequence"/>, удовлетворяющего предикату
        /// <paramref name="predicate"/>. Если предикату удовлетворяет несколько элементов, возвращается положение первого из них.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, по которой производится поиск.</param>
        /// <param name="predicate">Предикат, определяющий искомый элемент.</param>
        /// <returns>Положение элемента в последовательности, если он найден; <see cref="NotFound"/> (-1) в противном случае.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate"/><c> == null</c>.
        /// </exception>
        [Pure]
        public static int IndexOf<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence, [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var index = 0;
            foreach (var item in sequence)
            {
                if (predicate(item))
                    return index;
                index++;
            }
            return NotFound;
        }

        [Pure]
        public static bool SetEqual<T>([NotNull, InstantHandle] this IEnumerable<T> first, [NotNull, InstantHandle] IEnumerable<T> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return new HashSet<T>(first).SetEquals(second);
        }

        /// <summary>
        /// Частично сортирует по возрастанию <paramref name="source"/> по ключу <paramref name="keySelector"/> и
        /// возвращает первые <paramref name="count"/> отсортированных элементов.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="keySelector">Функция получения ключа сортировки из элементов последовательности.</param>
        /// <param name="count">Количество отсортированных элементов, которые надо вернуть.</param>
        [NotNull, Pure]
        public static IEnumerable<TItem> PartialOrderBy<TItem, TKey>(
            [NotNull] this IEnumerable<TItem> source,
            [NotNull] Func<TItem, TKey> keySelector,
            int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return new PartialOrderedEnumerable<TItem, TKey>(source, keySelector, null, false, count);
        }

        /// <summary>
        /// Частично сортирует по возрастанию <paramref name="source"/> по ключу <paramref name="keySelector"/>
        /// с помощью <paramref name="comparer"/> и возвращает первые <paramref name="count"/> отсортированных элементов.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="keySelector">Функция получения ключа сортировки из элементов последовательности.</param>
        /// <param name="comparer">Способ сравнения ключей, по которым выполняется сортировка.</param>
        /// <param name="count">Количество отсортированных элементов, которые надо вернуть.</param>
        [NotNull, Pure]
        public static IEnumerable<TItem> PartialOrderBy<TItem, TKey>(
            [NotNull] this IEnumerable<TItem> source,
            [NotNull] Func<TItem, TKey> keySelector,
            [NotNull] IComparer<TKey> comparer,
            int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new PartialOrderedEnumerable<TItem, TKey>(source, keySelector, comparer, false, count);
        }

        /// <summary>
        /// Частично сортирует по убыванию <paramref name="source"/> по ключу <paramref name="keySelector"/> и
        /// возвращает первые <paramref name="count"/> отсортированных элементов.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="keySelector">Функция получения ключа сортировки из элементов последовательности.</param>
        /// <param name="count">Количество отсортированных элементов, которые надо вернуть.</param>
        [NotNull, Pure]
        public static IEnumerable<TItem> PartialOrderByDescending<TItem, TKey>(
            [NotNull] this IEnumerable<TItem> source,
            [NotNull] Func<TItem, TKey> keySelector,
            int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return new PartialOrderedEnumerable<TItem, TKey>(source, keySelector, null, true, count);
        }

        /// <summary>
        /// Частично сортирует по убыванию <paramref name="source"/> по ключу <paramref name="keySelector"/>
        /// с помощью <paramref name="comparer"/> и возвращает первые <paramref name="count"/> отсортированных элементов.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="keySelector">Функция получения ключа сортировки из элементов последовательности.</param>
        /// <param name="comparer">Способ сравнения ключей, по которым выполняется сортировка.</param>
        /// <param name="count">Количество отсортированных элементов, которые надо вернуть.</param>
        [NotNull, Pure]
        public static IEnumerable<TItem> PartialOrderByDescending<TItem, TKey>(
            [NotNull] this IEnumerable<TItem> source,
            [NotNull] Func<TItem, TKey> keySelector,
            [NotNull] IComparer<TKey> comparer,
            int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new PartialOrderedEnumerable<TItem, TKey>(source, keySelector, comparer, true, count);
        }

        private sealed class PartialOrderedEnumerable<TItem, TKey> : IEnumerable<TItem>
        {
            #region Fields
            [NotNull]
            private readonly IEnumerable<TItem> _source;

            [NotNull]
            private readonly Func<TItem, TKey> _keySelector;

            [NotNull]
            private readonly IComparer<TKey> _comparer;

            private readonly bool _descending;
            private readonly int _count;
            #endregion

            #region Ctor
            internal PartialOrderedEnumerable(
                [NotNull] IEnumerable<TItem> source,
                [NotNull] Func<TItem, TKey> keySelector,
                [CanBeNull] IComparer<TKey> comparer,
                bool descending, int count)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);

                this._source = source;
                this._keySelector = keySelector;
                this._comparer = comparer ?? Comparer<TKey>.Default;
                this._descending = descending;
                this._count = count;
            }
            #endregion

            #region Methods
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<TItem> GetEnumerator()
            {
                if (_count <= 0)
                    yield break;

                (TKey Key, TItem Item)[] keysWithItems;

                if (_source is ICollection<TItem> collection)
                {
                    keysWithItems = new (TKey, TItem)[collection.Count];

                    var index = 0;
                    foreach (var item in collection)
                    {
                        keysWithItems[index] = (_keySelector(item), item);
                        index++;
                    }
                }
                else
                {
                    keysWithItems = _source.Select(item => (_keySelector(item), item)).ToArray();
                }

                if (keysWithItems.Length == 0)
                    yield break;

                PartialSort(keysWithItems, 0, keysWithItems.Length - 1, _count);

                var upper = Math.Min(_count, keysWithItems.Length);
                for (var i = 0; i < upper; i++)
                    yield return keysWithItems[i].Item;
            }

            private int CompareKeys(TKey left, TKey right) => _descending
                ? _comparer.Compare(right, left)
                : _comparer.Compare(left, right);

            private void PartialSort((TKey Key, TItem Item)[] keysWithItems, int left, int right, int itemsToSort)
            {
                if (itemsToSort <= 0)
                    return;

                var i = left;
                var j = right;
                var pivot = keysWithItems[(left + right) / 2].Item1;

                while (i <= j)
                {
                    while (CompareKeys(keysWithItems[i].Key, pivot) < 0)
                        i++;

                    while (CompareKeys(keysWithItems[j].Key, pivot) > 0)
                        j--;

                    if (i <= j)
                    {
                        if (i < j)
                        {
                            var tmp = keysWithItems[i];
                            keysWithItems[i] = keysWithItems[j];
                            keysWithItems[j] = tmp;
                        }

                        i++;
                        j--;
                    }
                }

                if (left < j)
                    PartialSort(keysWithItems, left, j, itemsToSort);

                if (i < right)
                    PartialSort(keysWithItems, i, right, left + itemsToSort - i);
            }
            #endregion
        }

        [NotNull]
        public static IEnumerable<IEnumerable<T>> GetCartesianProduct<T>(
            [NotNull] this IEnumerable<IEnumerable<T>> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(
                EnumerableEx.Return(Enumerable.Empty<T>()),
                (accumulator, sequence) =>
                    from acc in accumulator
                    from item in sequence
                    select acc.Concat(EnumerableEx.Return(item)));
        }

        public static IEnumerable<(Maybe<TOuter> Outer, Maybe<TInner> Inner)> FullOuterJoin<TOuter, TInner, TKey>(
            [NotNull] this IEnumerable<TOuter> outer,
            [NotNull] IEnumerable<TInner> inner,
            [NotNull] Func<TOuter, TKey> outerKeySelector,
            [NotNull] Func<TInner, TKey> innerKeySelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));

            return FullOuterJoinCore();

            IEnumerable<(Maybe<TOuter>, Maybe<TInner>)> FullOuterJoinCore()
            {
                var groupedInner = inner.ToLookup(innerKeySelector);

                var joinedKeys = new HashSet<TKey>();

                foreach (var outerItem in outer)
                {
                    var outerKey = outerKeySelector(outerItem);

                    if (groupedInner.Contains(outerKey))
                    {
                        joinedKeys.Add(outerKey);

                        foreach (var innerItem in groupedInner[outerKey])
                        {
                            yield return (outerItem, innerItem);
                        }
                    }
                    else
                    {
                        yield return (outerItem, Maybe<TInner>.Empty);
                    }
                }

                foreach (var group in groupedInner)
                {
                    if (joinedKeys.Contains(group.Key))
                        continue;

                    foreach (var innerItem in group)
                    {
                        yield return (Maybe<TOuter>.Empty, innerItem);
                    }
                }
            }
        }

        public static IEnumerable<(TOuter Outer, Maybe<TInner> Inner)> LeftOuterJoin<TOuter, TInner, TKey>(
            [NotNull] this IEnumerable<TOuter> outer,
            [NotNull] IEnumerable<TInner> inner,
            [NotNull] Func<TOuter, TKey> outerKeySelector,
            [NotNull] Func<TInner, TKey> innerKeySelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));

            return LeftOuterJoinCore();

            IEnumerable<(TOuter, Maybe<TInner>)> LeftOuterJoinCore()
            {
                var groupedInner = inner.ToLookup(innerKeySelector);

                foreach (var outerItem in outer)
                {
                    var outerKey = outerKeySelector(outerItem);

                    if (groupedInner.Contains(outerKey))
                    {
                        foreach (var innerItem in groupedInner[outerKey])
                        {
                            yield return (outerItem, innerItem);
                        }
                    }
                    else
                    {
                        yield return (outerItem, Maybe<TInner>.Empty);
                    }
                }
            }
        }

        public static IEnumerable<(Maybe<TOuter> Outer, TInner Inner)> RightOuterJoin<TOuter, TInner, TKey>(
            [NotNull] this IEnumerable<TOuter> outer,
            [NotNull] IEnumerable<TInner> inner,
            [NotNull] Func<TOuter, TKey> outerKeySelector,
            [NotNull] Func<TInner, TKey> innerKeySelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));

            return RightOuterJoinCore();

            IEnumerable<(Maybe<TOuter>, TInner)> RightOuterJoinCore()
            {
                var groupedInner = inner.ToLookup(innerKeySelector);

                var joinedKeys = new HashSet<TKey>();

                foreach (var outerItem in outer)
                {
                    var outerKey = outerKeySelector(outerItem);

                    if (groupedInner.Contains(outerKey))
                    {
                        joinedKeys.Add(outerKey);

                        foreach (var innerItem in groupedInner[outerKey])
                        {
                            yield return (outerItem, innerItem);
                        }
                    }
                }

                foreach (var group in groupedInner)
                {
                    if (joinedKeys.Contains(group.Key))
                        continue;

                    foreach (var innerItem in group)
                    {
                        yield return (Maybe<TOuter>.Empty, innerItem);
                    }
                }
            }
        }

        [NotNull]
        public static IEnumerable<(TLeft Left, TRight Right)> Zip<TLeft, TRight>(
            [NotNull] this IEnumerable<TLeft> left,
            [NotNull] IEnumerable<TRight> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return left.Zip(right, (l, r) => (l, r));
        }

        [NotNull]
        public static IEnumerable<(Maybe<TLeft> Left, Maybe<TRight> Right)> ZipAll<TLeft, TRight>(
            [NotNull] this IEnumerable<TLeft> left,
            [NotNull] IEnumerable<TRight> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            using (var leftEnum = left.GetEnumerator())
            using (var rightEnum = right.GetEnumerator())
            {
                while (true)
                {
                    var hasLeft = leftEnum.MoveNext();
                    var hasRight = rightEnum.MoveNext();

                    if (hasLeft && hasRight)
                    {
                        yield return (leftEnum.Current, rightEnum.Current);
                    }
                    else if (hasLeft)
                    {
                        yield return (leftEnum.Current, Maybe<TRight>.Empty);

                        while (leftEnum.MoveNext())
                            yield return (leftEnum.Current, Maybe<TRight>.Empty);
                    }
                    else if (hasRight)
                    {
                        yield return (Maybe<TLeft>.Empty, rightEnum.Current);

                        while (leftEnum.MoveNext())
                            yield return (Maybe<TLeft>.Empty, rightEnum.Current);
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }

        [NotNull, ItemNotNull]
        public static IEnumerable<IEnumerable<T>> GetAllCombinations<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return GetAllCombinationsCore(source.ToList());

            IEnumerable<IEnumerable<T>> GetAllCombinationsCore(IReadOnlyList<T> items)
            {
                if (items.Count == 0)
                    yield break;

                var head = items[0];
                yield return new[] { head };

                var tail = items.Skip(1).ToList();

                foreach (var tailCombination in GetAllCombinationsCore(tail))
                {
                    yield return new[] { head }.Concat(tailCombination);
                }

                foreach (var tailCombination in GetAllCombinationsCore(tail))
                {
                    yield return tailCombination;
                }
            }
        }
    }
}