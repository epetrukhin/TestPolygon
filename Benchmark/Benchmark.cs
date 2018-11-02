using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Benchmark
{
    // ReSharper disable once ClassCanBeSealed.Global
    [Config(typeof(Config))]
    public class Benchmark
    {
        // ReSharper disable once EmptyConstructor
        public Benchmark()
        {}

        private readonly List<int> _list = new List<int>();
        private readonly LinkedList<int> _linkedList = new LinkedList<int>();

        [Benchmark(Baseline = true, OperationsPerInvoke = 1000)]
        public void List()
        {
            for (int i = 0; i < 1000; i++)
            {
                _list.Insert(0, 42);
            }
        }

        [Benchmark(OperationsPerInvoke = 1000)]
        public void LinkedList()
        {
            for (int i = 0; i < 1000; i++)
            {
                _linkedList.AddFirst(42);
            }
        }
    }
}