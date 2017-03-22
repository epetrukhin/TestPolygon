using System;
using System.Collections.Generic;

using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Functional
{
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
            private readonly TLeft value;
            #endregion

            #region Ctor
            public Left(TLeft value) => this.value = value;
            #endregion

            #region Methods
            public override TResult Case<TResult>(Func<TLeft, TResult> ofLeft, Func<TRight, TResult> ofRight)
            {
                if (ofLeft == null)
                    throw new ArgumentNullException(nameof(ofLeft));
                if (ofRight == null)
                    throw new ArgumentNullException(nameof(ofRight));

                return ofLeft(value);
            }

            public override void Case(Action<TLeft> ofLeft, Action<TRight> ofRight)
            {
                if (ofLeft == null)
                    throw new ArgumentNullException(nameof(ofLeft));
                if (ofRight == null)
                    throw new ArgumentNullException(nameof(ofRight));

                ofLeft(value);
            }

	        // ReSharper disable once CompareNonConstrainedGenericWithNull
            public override string ToString() => value == null ? "Left(null)" : $"Left({value})";
            #endregion

            #region Equality
            public override bool Equals([CanBeNull] Either<TLeft, TRight> other)
            {
                var left = other as Left;

                return
                    left != null &&
                    EqualityComparer<TLeft>.Default.Equals(value, left.value);
            }

            public override int GetHashCode() => EqualityComparer<TLeft>.Default.GetHashCode(value);
            #endregion
        }

        private sealed class Right : Either<TLeft, TRight>
        {
            #region Fields
            private readonly TRight value;
            #endregion

            #region Ctor
            public Right(TRight value) => this.value = value;
            #endregion

            #region Methods
            public override TResult Case<TResult>(Func<TLeft, TResult> ofLeft, Func<TRight, TResult> ofRight)
            {
                if (ofLeft == null)
                    throw new ArgumentNullException(nameof(ofLeft));
                if (ofRight == null)
                    throw new ArgumentNullException(nameof(ofRight));

                return ofRight(value);
            }

            public override void Case(Action<TLeft> ofLeft, Action<TRight> ofRight)
            {
                if (ofLeft == null)
                    throw new ArgumentNullException(nameof(ofLeft));
                if (ofRight == null)
                    throw new ArgumentNullException(nameof(ofRight));

                ofRight(value);
            }

	        // ReSharper disable once CompareNonConstrainedGenericWithNull
            public override string ToString() => value == null ? "Right(null)" : $"Right({value})";
            #endregion

            #region Equality
            public override bool Equals([CanBeNull] Either<TLeft, TRight> other)
            {
                var right = other as Right;

                return
                    right != null &&
                    EqualityComparer<TRight>.Default.Equals(value, right.value);
            }

            public override int GetHashCode() => EqualityComparer<TRight>.Default.GetHashCode(value);
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
        public sealed override bool Equals([CanBeNull] object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return
                obj is Either<TLeft, TRight> &&
                Equals((Either<TLeft, TRight>)obj);
        }

        public abstract override int GetHashCode();

        public abstract bool Equals([CanBeNull] Either<TLeft, TRight> other);

        public static bool operator ==(Either<TLeft, TRight> left, Either<TLeft, TRight> right) => Equals(left, right);

        public static bool operator !=(Either<TLeft, TRight> left, Either<TLeft, TRight> right) => !Equals(left, right);
        #endregion

        #endregion
    }
}