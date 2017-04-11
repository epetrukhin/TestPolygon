using System;
using System.Collections.Generic;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Functional
{
    /// <summary>
    /// Тип-обёртка для значений. Допускает отсутствие значения (<see cref="Empty"/>).
    /// В отличие от <see cref="Nullable"/> можно обернуть значение reference-типа.
    /// </summary>
    /// <typeparam name="T">Тип обёрнутого значения</typeparam>
    [PublicAPI]
    public struct Maybe<T> : IEquatable<Maybe<T>>
    {
        #region Fields
        private readonly T _value;
        #endregion

        #region Ctor
        public Maybe([CanBeNull] T value)
        {
            HasValue = true;
            _value = value;
        }
        #endregion

        #region Props
        /// <summary>
        /// Отсутствующее значение для типа <typeparamref name="T" />.
        /// </summary>
        public static Maybe<T> Empty => new Maybe<T>();

        /// <summary>
        /// Возвращает флаг наличия значения.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Возвращает флаг отсутствия значения.
        /// </summary>
        public bool IsEmpty => !HasValue;

        /// <summary>
        /// Обёрнутое значение.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Значение отсутствует.
        /// </exception>
        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException();

                return _value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Возвращает this, если есть значение или <paramref name="other"/>, если значение отсутствует.
        /// </summary>
        public Maybe<T> Or(Maybe<T> other) => HasValue ? this : other;

        /// <summary>
        /// Возвращает <see cref="Value"/>, если есть значение или значение по умолчанию для <typeparamref name="T"/>.
        /// </summary>
        [CanBeNull]
        public T GetValueOrDefault() => _value;

        /// <summary>
        /// Возвращает <see cref="Value"/>, если есть значение или <paramref name="defaultValue"/>.
        /// </summary>
        [CanBeNull]
        public T GetValueOrDefault([CanBeNull] T defaultValue) => HasValue ? _value : defaultValue;

        public static implicit operator Maybe<T>([CanBeNull] T value) => new Maybe<T>(value);

        public override string ToString()
        {
            if (!HasValue)
                return "Maybe.Empty";

            // ReSharper disable once CompareNonConstrainedGenericWithNull
            return _value == null ? "Maybe(null)" : $"Maybe({_value})";
        }

        #region Equality
        public bool Equals(Maybe<T> other) =>
            HasValue
                ? other.HasValue && EqualityComparer<T>.Default.Equals(_value, other._value)
                : !other.HasValue;

        public override bool Equals([CanBeNull] object obj) =>
            obj is Maybe<T> && Equals((Maybe<T>)obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return (HasValue.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(_value);
            }
        }

        public static bool operator ==(Maybe<T> left, Maybe<T> right) => left.Equals(right);
        public static bool operator !=(Maybe<T> left, Maybe<T> right) => !left.Equals(right);
        #endregion
        #endregion
    }

    [PublicAPI]
    internal static class Maybe
    {
        public static Maybe<TOut> Map<TIn, TOut>(this Maybe<TIn> value, [NotNull] Func<TIn, TOut> map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            return map.WrapFunction()(value);
        }

        [NotNull]
        public static Func<Maybe<TIn>, Maybe<TOut>> WrapFunction<TIn, TOut>([NotNull] this Func<TIn, TOut> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return
                input => input.HasValue
                    ? func(input.Value)
                    : Maybe<TOut>.Empty;
        }

        [NotNull]
        public static Func<Maybe<TIn1>, Maybe<TIn2>, Maybe<TOut>> WrapFunction<TIn1, TIn2, TOut>([NotNull] this Func<TIn1, TIn2, TOut> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return
                (input1, input2) => input1.HasValue && input2.HasValue
                    ? func(input1.Value, input2.Value)
                    : Maybe<TOut>.Empty;
        }
    }
}