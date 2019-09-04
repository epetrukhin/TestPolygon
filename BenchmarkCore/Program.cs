using System;
using BenchmarkDotNet.Running;

namespace BenchmarkCore
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