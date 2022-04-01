using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Functional
{
    [PublicAPI]
    public readonly struct Result<TValue, TError> : IEquatable<Result<TValue, TError>>
    {
        private readonly TValue _value;
        private readonly TError _error;
        private readonly bool _success;

        private Result(TValue value, TError error, bool success)
        {
            _value = value;
            _error = error;
            _success = success;
        }

        public Result(TValue value)
            : this(value, default, true)
        {}

        public Result(TError error)
            : this(default, error, false)
        {}

        public static implicit operator Result<TValue, TError>(Result.SuccessResult<TValue> success) =>
            new(success.Value);

        public static implicit operator Result<TValue, TError>(TValue value) =>
            new(value);

        public static implicit operator Result<TValue, TError>(Result.FailResult<TError> fail) =>
            new(fail.Error);

        public static implicit operator Result<TValue, TError>(TError error) =>
            new(error);

        public bool IsSuccess => _success;

        public bool IsFail => !_success;

        public TValue Value
        {
            get
            {
                if (!_success)
                    throw new InvalidOperationException();

                return _value;
            }
        }

        public TError Error
        {
            get
            {
                if (_success)
                    throw new InvalidOperationException();

                return _error;
            }
        }

        [Pure]
        public TValue GetValueOrDefault() => _value;

        [Pure]
        public TValue GetValueOrDefault(TValue @default) => _success ? _value : @default;

        [Pure]
        public Result<TValueEx, TError> MapValue<TValueEx>([NotNull] Func<TValue, TValueEx> mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            return _success
                ? new Result<TValueEx, TError>(mapper(_value))
                : new Result<TValueEx, TError>(_error);
        }

        [NotNull, Pure]
        public Task<Result<TValueEx, TError>> MapValue<TValueEx>([NotNull] Func<TValue, Task<TValueEx>> mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            return MapValueCore(this, mapper);

            async Task<Result<TValueEx, TError>> MapValueCore(Result<TValue, TError> source, Func<TValue, Task<TValueEx>> map) =>
                source._success
                    ? new Result<TValueEx, TError>(await map(source._value))
                    : new Result<TValueEx, TError>(source._error);
        }

        [Pure]
        public Result<TValue, TErrorEx> MapError<TErrorEx>([NotNull] Func<TError, TErrorEx> mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            return _success
                ? new Result<TValue, TErrorEx>(_value)
                : new Result<TValue, TErrorEx>(mapper(_error));
        }

        #region Equality
        public bool Equals(Result<TValue, TError> other)
        {
            if (_success != other._success)
                return false;

            return _success
                ? EqualityComparer<TValue>.Default.Equals(_value, other._value)
                : EqualityComparer<TError>.Default.Equals(_error, other._error);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Result<TValue, TError> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return
                    _success.GetHashCode() * 397 ^
                    (_success
                        ? EqualityComparer<TValue>.Default.GetHashCode(_value)
                        : EqualityComparer<TError>.Default.GetHashCode(_error));
            }
        }

        public static bool operator ==(Result<TValue, TError> left, Result<TValue, TError> right) => left.Equals(right);

        public static bool operator !=(Result<TValue, TError> left, Result<TValue, TError> right) => !left.Equals(right);
        #endregion
    }

    [PublicAPI]
    public static class Result
    {
        [PublicAPI]
        public readonly struct SuccessResult<T> : IEquatable<SuccessResult<T>>
        {
            public T Value { get; }

            public SuccessResult(T value) => Value = value;

            [Pure]
            public Result<T, TError> WithErrorType<TError>() =>
                new(Value);

            #region Equality
            public bool Equals(SuccessResult<T> other) => EqualityComparer<T>.Default.Equals(Value, other.Value);

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                return obj is SuccessResult<T> other && Equals(other);
            }

            public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);

            public static bool operator ==(SuccessResult<T> left, SuccessResult<T> right) => left.Equals(right);

            public static bool operator !=(SuccessResult<T> left, SuccessResult<T> right) => !left.Equals(right);
            #endregion
        }

        [PublicAPI]
        public readonly struct FailResult<T> : IEquatable<FailResult<T>>
        {
            public T Error { get; }

            public FailResult(T error) => Error = error;

            [Pure]
            public Result<TValue, T> WithValueType<TValue>() =>
                new(Error);

            #region Equality
            public bool Equals(FailResult<T> other) => EqualityComparer<T>.Default.Equals(Error, other.Error);

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                return obj is FailResult<T> other && Equals(other);
            }

            public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Error);

            public static bool operator ==(FailResult<T> left, FailResult<T> right) => left.Equals(right);

            public static bool operator !=(FailResult<T> left, FailResult<T> right) => !left.Equals(right);
            #endregion
        }

        [Pure]
        public static SuccessResult<T> Success<T>(T value) => new(value);

        [Pure]
        public static FailResult<T> Fail<T>(T error) => new(error);

        #region Select & SelectMany
        [Pure]
        public static Result<TValueEx, TError> Select<TValue, TValueEx, TError>(
            this Result<TValue, TError> source,
            [NotNull] Func<TValue, TValueEx> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.MapValue(selector);
        }

        [Pure]
        public static Result<TValueEx, TError> SelectMany<TValue, TValueEx, TError>(
            this Result<TValue, TError> source,
            [NotNull] Func<TValue, Result<TValueEx, TError>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.IsSuccess
                ? selector(source.Value)
                : new Result<TValueEx, TError>(source.Error);
        }

        [Pure]
        public static Result<TValueExEx, TError> SelectMany<TValue, TValueEx, TValueExEx, TError>(
            this Result<TValue, TError> source,
            [NotNull] Func<TValue, Result<TValueEx, TError>> selector,
            [NotNull] Func<TValue, TValueEx, TValueExEx> projector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (projector == null)
                throw new ArgumentNullException(nameof(projector));

            if (source.IsFail)
                return new Result<TValueExEx, TError>(source.Error);

            var selected = selector(source.Value);
            return selected.IsSuccess
                ? new Result<TValueExEx, TError>(projector(source.Value, selected.Value))
                : new Result<TValueExEx, TError>(selected.Error);
        }
        #endregion

        #region Mapping for async result
        [NotNull, Pure]
        public static Task<Result<TValueEx, TError>> MapValue<TValue, TValueEx, TError>(
            [NotNull] this Task<Result<TValue, TError>> source,
            [NotNull] Func<TValue, TValueEx> mapper)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            return MapValueCore(source, mapper);

            async Task<Result<TValueEx, TError>> MapValueCore(Task<Result<TValue, TError>> src, Func<TValue, TValueEx> map) =>
                (await src).MapValue(map);
        }

        [NotNull, Pure]
        public static Task<Result<TValueEx, TError>> MapValue<TValue, TValueEx, TError>(
            [NotNull] this Task<Result<TValue, TError>> source,
            [NotNull] Func<TValue, Task<TValueEx>> mapper)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            return MapValueCore(source, mapper);

            async Task<Result<TValueEx, TError>> MapValueCore(Task<Result<TValue, TError>> src, Func<TValue, Task<TValueEx>> map) =>
                await (await src).MapValue(map);
        }

        [NotNull, Pure]
        public static Task<Result<TValue, TErrorEx>> MapError<TValue, TError, TErrorEx>(
            [NotNull] this Task<Result<TValue, TError>> source,
            [NotNull] Func<TError, TErrorEx> mapper)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            return MapErrorCore(source, mapper);

            async Task<Result<TValue, TErrorEx>> MapErrorCore(Task<Result<TValue, TError>> src, Func<TError, TErrorEx> map) =>
                (await src).MapError(map);
        }
        #endregion

        #region Select & SelectMany for async results
        [NotNull, Pure]
        public static Task<Result<TValueEx, TError>> Select<TValue, TValueEx, TError>(
            this Result<TValue, TError> source,
            [NotNull] Func<TValue, Task<TValueEx>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.MapValue(selector);
        }

        [NotNull, Pure]
        public static Task<Result<TValueEx, TError>> Select<TValue, TValueEx, TError>(
            [NotNull] this Task<Result<TValue, TError>> source,
            [NotNull] Func<TValue, TValueEx> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.MapValue(selector);
        }

        [NotNull, Pure]
        public static Task<Result<TValueEx, TError>> Select<TValue, TValueEx, TError>(
            [NotNull] this Task<Result<TValue, TError>> source,
            [NotNull] Func<TValue, Task<TValueEx>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.MapValue(selector);
        }

        [NotNull, Pure]
        public static Task<Result<TValueEx, TError>> SelectMany<TValue, TValueEx, TError>(
            this Result<TValue, TError> source,
            [NotNull] Func<TValue, Task<Result<TValueEx, TError>>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SelectManyCore(source, selector);

            async Task<Result<TValueEx, TError>> SelectManyCore(Result<TValue, TError> src, Func<TValue, Task<Result<TValueEx, TError>>> select) =>
                src.IsSuccess
                    ? await select(src.Value)
                    : new Result<TValueEx, TError>(src.Error);
        }

        [NotNull, Pure]
        public static Task<Result<TValueEx, TError>> SelectMany<TValue, TValueEx, TError>(
            [NotNull] this Task<Result<TValue, TError>> source,
            [NotNull] Func<TValue, Result<TValueEx, TError>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SelectManyCore(source, selector);

            async Task<Result<TValueEx, TError>> SelectManyCore(
                Task<Result<TValue, TError>> src,
                Func<TValue, Result<TValueEx, TError>> select)
            {
                var sourceResult = await src;
                return sourceResult.IsSuccess
                    ? select(sourceResult.Value)
                    : new Result<TValueEx, TError>(sourceResult.Error);
            }
        }

        [NotNull, Pure]
        public static Task<Result<TValueEx, TError>> SelectMany<TValue, TValueEx, TError>(
            [NotNull] this Task<Result<TValue, TError>> source,
            [NotNull] Func<TValue, Task<Result<TValueEx, TError>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SelectManyCore(source, selector);

            async Task<Result<TValueEx, TError>> SelectManyCore(
                Task<Result<TValue, TError>> src,
                Func<TValue, Task<Result<TValueEx, TError>>> select)
            {
                var sourceResult = await src;
                return sourceResult.IsSuccess
                    ? await select(sourceResult.Value)
                    : new Result<TValueEx, TError>(sourceResult.Error);
            }
        }

        [NotNull, Pure]
        public static Task<Result<TValueExEx, TError>> SelectMany<TValue, TValueEx, TValueExEx, TError>(
            this Result<TValue, TError> source,
            [NotNull] Func<TValue, Task<Result<TValueEx, TError>>> selector,
            [NotNull] Func<TValue, TValueEx, TValueExEx> projector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (projector == null)
                throw new ArgumentNullException(nameof(projector));

            return SelectManyCore(source, selector, projector);

            async Task<Result<TValueExEx, TError>> SelectManyCore(
                Result<TValue, TError> src,
                Func<TValue, Task<Result<TValueEx, TError>>> select,
                Func<TValue, TValueEx, TValueExEx> project)
            {
                if (src.IsFail)
                    return new Result<TValueExEx, TError>(src.Error);

                var selected = await select(src.Value);
                return selected.IsSuccess
                    ? new Result<TValueExEx, TError>(project(src.Value, selected.Value))
                    : new Result<TValueExEx, TError>(selected.Error);
            }
        }

        [NotNull, Pure]
        public static Task<Result<TValueExEx, TError>> SelectMany<TValue, TValueEx, TValueExEx, TError>(
            this Result<TValue, TError> source,
            [NotNull] Func<TValue, Result<TValueEx, TError>> selector,
            [NotNull] Func<TValue, TValueEx, Task<TValueExEx>> projector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (projector == null)
                throw new ArgumentNullException(nameof(projector));

            return SelectManyCore(source, selector, projector);

            async Task<Result<TValueExEx, TError>> SelectManyCore(
                Result<TValue, TError> src,
                Func<TValue, Result<TValueEx, TError>> select,
                Func<TValue, TValueEx, Task<TValueExEx>> project)
            {
                if (src.IsFail)
                    return new Result<TValueExEx, TError>(src.Error);

                var selected = select(src.Value);
                return selected.IsSuccess
                    ? new Result<TValueExEx, TError>(await project(src.Value, selected.Value))
                    : new Result<TValueExEx, TError>(selected.Error);
            }
        }

        [NotNull, Pure]
        public static Task<Result<TValueExEx, TError>> SelectMany<TValue, TValueEx, TValueExEx, TError>(
            this Result<TValue, TError> source,
            [NotNull] Func<TValue, Task<Result<TValueEx, TError>>> selector,
            [NotNull] Func<TValue, TValueEx, Task<TValueExEx>> projector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (projector == null)
                throw new ArgumentNullException(nameof(projector));

            return SelectManyCore(source, selector, projector);

            async Task<Result<TValueExEx, TError>> SelectManyCore(
                Result<TValue, TError> src,
                Func<TValue, Task<Result<TValueEx, TError>>> select,
                Func<TValue, TValueEx, Task<TValueExEx>> project)
            {
                if (src.IsFail)
                    return new Result<TValueExEx, TError>(src.Error);

                var selected = await select(src.Value);
                return selected.IsSuccess
                    ? new Result<TValueExEx, TError>(await project(src.Value, selected.Value))
                    : new Result<TValueExEx, TError>(selected.Error);
            }
        }

        [NotNull, Pure]
        public static Task<Result<TValueExEx, TError>> SelectMany<TValue, TValueEx, TValueExEx, TError>(
            [NotNull] this Task<Result<TValue, TError>> source,
            [NotNull] Func<TValue, Result<TValueEx, TError>> selector,
            [NotNull] Func<TValue, TValueEx, TValueExEx> projector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (projector == null)
                throw new ArgumentNullException(nameof(projector));

            return SelectManyCore(source, selector, projector);

            async Task<Result<TValueExEx, TError>> SelectManyCore(
                Task<Result<TValue, TError>> src,
                Func<TValue, Result<TValueEx, TError>> select,
                Func<TValue, TValueEx, TValueExEx> project)
            {
                var sourceResult = await src;
                if (sourceResult.IsFail)
                    return new Result<TValueExEx, TError>(sourceResult.Error);

                var selected = select(sourceResult.Value);
                return selected.IsSuccess
                    ? new Result<TValueExEx, TError>(project(sourceResult.Value, selected.Value))
                    : new Result<TValueExEx, TError>(selected.Error);
            }
        }

        [NotNull, Pure]
        public static Task<Result<TValueExEx, TError>> SelectMany<TValue, TValueEx, TValueExEx, TError>(
            [NotNull] this Task<Result<TValue, TError>> source,
            [NotNull] Func<TValue, Task<Result<TValueEx, TError>>> selector,
            [NotNull] Func<TValue, TValueEx, TValueExEx> projector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (projector == null)
                throw new ArgumentNullException(nameof(projector));

            return SelectManyCore(source, selector, projector);

            async Task<Result<TValueExEx, TError>> SelectManyCore(
                Task<Result<TValue, TError>> src,
                Func<TValue, Task<Result<TValueEx, TError>>> select,
                Func<TValue, TValueEx, TValueExEx> project)
            {
                var sourceResult = await src;
                if (sourceResult.IsFail)
                    return new Result<TValueExEx, TError>(sourceResult.Error);

                var selected = await select(sourceResult.Value);
                return selected.IsSuccess
                    ? new Result<TValueExEx, TError>(project(sourceResult.Value, selected.Value))
                    : new Result<TValueExEx, TError>(selected.Error);
            }
        }

        [NotNull, Pure]
        public static Task<Result<TValueExEx, TError>> SelectMany<TValue, TValueEx, TValueExEx, TError>(
            [NotNull] this Task<Result<TValue, TError>> source,
            [NotNull] Func<TValue, Result<TValueEx, TError>> selector,
            [NotNull] Func<TValue, TValueEx, Task<TValueExEx>> projector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (projector == null)
                throw new ArgumentNullException(nameof(projector));

            return SelectManyCore(source, selector, projector);

            async Task<Result<TValueExEx, TError>> SelectManyCore(
                Task<Result<TValue, TError>> src,
                Func<TValue, Result<TValueEx, TError>> select,
                Func<TValue, TValueEx, Task<TValueExEx>> project)
            {
                var sourceResult = await src;
                if (sourceResult.IsFail)
                    return new Result<TValueExEx, TError>(sourceResult.Error);

                var selected = select(sourceResult.Value);
                return selected.IsSuccess
                    ? new Result<TValueExEx, TError>(await project(sourceResult.Value, selected.Value))
                    : new Result<TValueExEx, TError>(selected.Error);
            }
        }

        [NotNull, Pure]
        public static Task<Result<TValueExEx, TError>> SelectMany<TValue, TValueEx, TValueExEx, TError>(
            [NotNull] this Task<Result<TValue, TError>> source,
            [NotNull] Func<TValue, Task<Result<TValueEx, TError>>> selector,
            [NotNull] Func<TValue, TValueEx, Task<TValueExEx>> projector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (projector == null)
                throw new ArgumentNullException(nameof(projector));

            return SelectManyCore(source, selector, projector);

            async Task<Result<TValueExEx, TError>> SelectManyCore(
                Task<Result<TValue, TError>> src,
                Func<TValue, Task<Result<TValueEx, TError>>> select,
                Func<TValue, TValueEx, Task<TValueExEx>> project)
            {
                var sourceResult = await src;
                if (sourceResult.IsFail)
                    return new Result<TValueExEx, TError>(sourceResult.Error);

                var selected = await select(sourceResult.Value);
                return selected.IsSuccess
                    ? new Result<TValueExEx, TError>(await project(sourceResult.Value, selected.Value))
                    : new Result<TValueExEx, TError>(selected.Error);
            }
        }
        #endregion
    }
}