using BenchmarkDotNet.Attributes;

namespace ConsoleApp
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class Benchmark
	{
		public Benchmark()
		{

		}

		[Benchmark(Baseline = true)]
		public int Foo() => 42;

		[Benchmark]
		public int Bar() => 42;
	}
}