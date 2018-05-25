using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;

namespace Benchmark
{
    public sealed class Config : ManualConfig
    {
        public Config()
        {
                Add(MemoryDiagnoser.Default);
                Add(StatisticColumn.P95);

            Add(
//                Job.LegacyJitX86
//                    .WithLaunchCount(1)
//                    .WithWarmupCount(1)
//                    .WithTargetCount(3),
                Job.LegacyJitX64
                    .WithLaunchCount(1)
                    .WithWarmupCount(1)
                    .WithTargetCount(3),
                Job.RyuJitX64
                    .WithLaunchCount(1)
                    .WithWarmupCount(1)
                    .WithTargetCount(3));
        }
    }
}