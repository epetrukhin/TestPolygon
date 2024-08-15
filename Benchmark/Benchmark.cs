#nullable enable

using System;
using System.Buffers;
using System.IO;
using BenchmarkDotNet.Attributes;

namespace Benchmark;

// ReSharper disable once ClassCanBeSealed.Global
[Config(typeof(Config))]
public class Benchmark
{
    private const int Count = 877;

    private static readonly SearchValues<char> Separators = SearchValues.Create(".");

    private static readonly string[] Bets = File.ReadAllLines(@"c:\Temp\bets.txt");

    [GlobalSetup]
    public void Setup()
    {}

    [Benchmark(Baseline = true, OperationsPerInvoke = Count)]
    public int StringContains()
    {
        var cnt = 0;
        foreach (var bet in Bets)
        {
            if (bet.Contains('.'))
                cnt++;
        }
        return cnt;
    }

    [Benchmark(OperationsPerInvoke = Count)]
    public int SearchValuesSeparators()
    {
        var cnt = 0;
        foreach (var bet in Bets)
        {
            if (bet.AsSpan().IndexOfAny(Separators) >= 0)
                cnt++;
        }
        return cnt;
    }
}