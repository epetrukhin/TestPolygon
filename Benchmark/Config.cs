﻿using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace Benchmark;

public sealed class Config : ManualConfig
{
    public Config()
    {
        // AddHardwareCounters(HardwareCounter.CacheMisses, HardwareCounter.BranchInstructions, HardwareCounter.BranchMispredictions);
        AddDiagnoser(MemoryDiagnoser.Default);
        AddColumn(StatisticColumn.P95, StatisticColumn.OperationsPerSecond);

        AddJob(
            Job.Default.WithRuntime(CoreRuntime.Core80)
                .WithLaunchCount(1)
                .WithWarmupCount(3)
                .WithIterationCount(10));
    }
}