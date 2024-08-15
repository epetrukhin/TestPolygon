using System;
using System.Collections.Generic;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Functional;

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
    /// Отсутствующее значение для типа<typeparamref name="T" />.
    /// </summary>
    public static Maybe<T> Empty => new();

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
    /// Возвращает this, если есть значение или<paramref name="other"/>, если значение отсутствует.
    /// </summary>
    public Maybe<T> Or(Maybe<T> other) => HasValue ? this : other;

    /// <summary>
    /// Возвращает<see cref="Value"/>, если есть значение или значение по умолчанию для <typeparamref name="T"/>.
    /// </summary>
    [CanBeNull]
    public T GetValueOrDefault() => _value;

    /// <summary>
    /// Возвращает<see cref="Value"/>, если есть значение или<paramref name="defaultValue"/>.
    /// </summary>
    [CanBeNull]
    public T GetValueOrDefault([CanBeNull] T defaultValue) => HasValue ? _value : defaultValue;

    public static implicit operator Maybe<T>([CanBeNull] T value) => new(value);

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

    public override bool Equals(object obj) =>
        obj is Maybe<T> maybe && Equals(maybe);

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
        ArgumentNullException.ThrowIfNull(map);

        return map.WrapFunction()(value);
    }

    [NotNull]
    public static Func<Maybe<TIn>, Maybe<TOut>> WrapFunction<TIn, TOut>([NotNull] this Func<TIn, TOut> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return
            input => input.HasValue
                ? func(input.Value)
                : Maybe<TOut>.Empty;
    }

    [NotNull]
    public static Func<Maybe<TIn1>, Maybe<TIn2>, Maybe<TOut>> WrapFunction<TIn1, TIn2, TOut>([NotNull] this Func<TIn1, TIn2, TOut> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return
            (input1, input2) => input1.HasValue && input2.HasValue
                ? func(input1.Value, input2.Value)
                : Maybe<TOut>.Empty;
    }
}

[PublicAPI]
public static class MaybeExtensions
{
    public static Maybe<TOut> Select<T, TOut>(this Maybe<T> source, [NotNull] Func<T, TOut> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return source.Map(selector);
    }

    public static Maybe<TOut> SelectMany<T, TOut>(this Maybe<T> source, [NotNull] Func<T, Maybe<TOut>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return source.HasValue ? selector(source.Value) : Maybe<TOut>.Empty;
    }

    public static Maybe<TOut> SelectMany<T, TColl, TOut>(this Maybe<T> source, [NotNull] Func<T, Maybe<TColl>> selector, [NotNull] Func<T, TColl, TOut> projector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentNullException.ThrowIfNull(projector);

        if (source.IsEmpty)
            return Maybe<TOut>.Empty;

        var selected = selector(source.Value);
        if (selected.IsEmpty)
            return Maybe<TOut>.Empty;

        return projector(source.Value, selected.Value);
    }

    public static Maybe<T> Where<T>(this Maybe<T> source, [NotNull] Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (source.IsEmpty)
            return source;

        if (!predicate(source.Value))
            return Maybe<T>.Empty;

        return source;
    }
}