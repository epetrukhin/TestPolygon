using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace BenchmarkCore
{
    // ReSharper disable once ClassCanBeSealed.Global
    [Config(typeof(Config))]
    public class Benchmark
    {
        private int[] _values;
        private const int Count = 128;

        [GlobalSetup]
        public void Setup()
        {
            var rnd = new Random(42);
            _values = new int[Count];
            for (var i = 0; i < Count; i++)
                _values[i] = rnd.Next();
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = Count)]
        public int Delegate()
        {
            Func<int, int> func = x => x + 42;

            var acc = 0;
            foreach (var x in _values)
            {
                acc += Foo.Add42ViaDelegate(x, func);
            }

            return acc;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public int Concept()
        {
            var acc = 0;
            foreach (var x in _values)
            {
                acc += Foo.Add42ViaConcept<Add42>(x);
            }

            return acc;
        }
    }

    public interface IAdd<T>
    {
        T Invoke(T value);
    }

    public readonly struct Add42 : IAdd<int>
    {
        public int Invoke(int x) => x + 42;
    }

    public static class Foo
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int Add42ViaDelegate(int x, Func<int, int> add42) =>
            add42(x);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int Add42ViaConcept<T>(int x)
            where T : struct, IAdd<int> =>
            default(T).Invoke(x);
    }
}