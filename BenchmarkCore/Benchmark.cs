using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace BenchmarkCore
{
    // ReSharper disable once ClassCanBeSealed.Global
    [Config(typeof(Config))]
    public class Benchmark
    {
        private const int VALUE = 1;

        private IEnumerable<int> _repeatSource;

        [Params(1000, 10000, 100000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            _repeatSource = Enumerable.Repeat(0, N).Select(x => VALUE);
        }

        [Benchmark]
        public List<int> ToListFromRepeat()
        {
            return _repeatSource.ToList();
        }

        [Benchmark]
        public int[] ToArrayFromRepeat()
        {
            return _repeatSource.ToArray();
        }
    }
}