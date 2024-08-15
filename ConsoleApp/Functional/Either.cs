using System;
using System.Collections.Generic;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Functional;

/// <summary>
/// Тип-обёртка для одного из двух значений, левого или правого.
/// </summary>
/// <typeparam name="TLeft">Тип левого значения.</typeparam>
/// <typeparam name="TRight">Тип правого значения.</typeparam>
[PublicAPI]
public abstract class Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>
{
    #region Inner Types
    private sealed class Left : Either<TLeft, TRight>
    {
        #region Fields
        private readonly TLeft _value;
        #endregion

        #region Ctor
        public Left(TLeft value) => _value = value;
        #endregion

        #region Methods
        public override TResult Case<TResult>(Func<TLeft, TResult> ofLeft, Func<TRight, TResult> ofRight)
        {
            ArgumentNullException.ThrowIfNull(ofLeft);
            ArgumentNullException.ThrowIfNull(ofRight);

            return ofLeft(_value);
        }

        public override void Case(Action<TLeft> ofLeft, Action<TRight> ofRight)
        {
            ArgumentNullException.ThrowIfNull(ofLeft);
            ArgumentNullException.ThrowIfNull(ofRight);

            ofLeft(_value);
        }

        // ReSharper disable once CompareNonConstrainedGenericWithNull
        public override string ToString() => _value == null ? "Left(null)" : $"Left({_value})";
        #endregion

        #region Equality
        public override bool Equals(Either<TLeft, TRight> other)
        {
            var left = other as Left;

            return
                left != null &&
                EqualityComparer<TLeft>.Default.Equals(_value, left._value);
        }

        public override int GetHashCode() => EqualityComparer<TLeft>.Default.GetHashCode(_value);
        #endregion
    }

    private sealed class Right : Either<TLeft, TRight>
    {
        #region Fields
        private readonly TRight _value;
        #endregion

        #region Ctor
        public Right(TRight value) => _value = value;
        #endregion

        #region Methods
        public override TResult Case<TResult>(Func<TLeft, TResult> ofLeft, Func<TRight, TResult> ofRight)
        {
            ArgumentNullException.ThrowIfNull(ofLeft);
            ArgumentNullException.ThrowIfNull(ofRight);

            return ofRight(_value);
        }

        public override void Case(Action<TLeft> ofLeft, Action<TRight> ofRight)
        {
            ArgumentNullException.ThrowIfNull(ofLeft);
            ArgumentNullException.ThrowIfNull(ofRight);

            ofRight(_value);
        }

        // ReSharper disable once CompareNonConstrainedGenericWithNull
        public override string ToString() => _value == null ? "Right(null)" : $"Right({_value})";
        #endregion

        #region Equality
        public override bool Equals(Either<TLeft, TRight> other)
        {
            var right = other as Right;

            return
                right != null &&
                EqualityComparer<TRight>.Default.Equals(_value, right._value);
        }

        public override int GetHashCode() => EqualityComparer<TRight>.Default.GetHashCode(_value);
        #endregion
    }
    #endregion

    #region Ctor & Factory Methods
    private Either()
    {}

    [NotNull]
    public static Either<TLeft, TRight> CreateLeft(TLeft value) => new Left(value);

    [NotNull]
    public static Either<TLeft, TRight> CreateRight(TRight value) => new Right(value);
    #endregion

    #region Methods
    /// <summary>
    /// В зависимости от того, какое значение хранится, вызывает соответствующую функцию и возвращает результат.
    /// </summary>
    /// <typeparam name="TResult">Тип возвращаемого значения.</typeparam>
    /// <param name="ofLeft">Функция для левого значения.</param>
    /// <param name="ofRight">Функция для правого значения.</param>
    /// <returns>Результат, который вернула вызванная функция.</returns>
    public abstract TResult Case<TResult>([NotNull] Func<TLeft, TResult> ofLeft, [NotNull] Func<TRight, TResult> ofRight);

    /// <summary>
    /// В зависимости от того, какое значение хранится, вызывает соответствующее действие.
    /// </summary>
    /// <param name="ofLeft">Действие для левого значения.</param>
    /// <param name="ofRight">Действие для правого значения.</param>
    public abstract void Case([NotNull] Action<TLeft> ofLeft, [NotNull] Action<TRight> ofRight);

    #region Equality
    public sealed override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;

        return
            obj is Either<TLeft, TRight> either &&
            Equals(either);
    }

    public abstract override int GetHashCode();

    public abstract bool Equals(Either<TLeft, TRight> other);

    public static bool operator ==(Either<TLeft, TRight> left, Either<TLeft, TRight> right) => Equals(left, right);

    public static bool operator !=(Either<TLeft, TRight> left, Either<TLeft, TRight> right) => !Equals(left, right);
    #endregion
    #endregion
}