using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Benchmark
{
    // ReSharper disable once ClassCanBeSealed.Global
    [Config(typeof(Config))]
    public partial class Benchmark
    {
        // ReSharper disable once EmptyConstructor
        public Benchmark()
        {
            _data = new double[10_000];
            var rnd = new Random();
            for (var i = 0; i < _data.Length; i++)
            {
                _data[i] = rnd.NextDouble();
            }
        }

        private readonly double[] _data;

        [Benchmark(Baseline = true)]
        public double LinqSum() => _data.Sum();

        [Benchmark]
        public double ForSum()
        {
            var result = 0d;
            for (var i = 0; i < _data.Length; i++)
            {
                result += _data[i];
            }
            return result;
        }
    }
}