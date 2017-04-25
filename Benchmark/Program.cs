using System;
using BenchmarkDotNet.Running;

namespace Benchmark
{
    internal static class Program
    {
        private static void Main()
        {
            var result = BenchmarkRunner.Run<Benchmark>();

            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}