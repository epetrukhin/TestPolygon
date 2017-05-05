using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Benchmark
{
    public sealed class Config : ManualConfig
    {
        public Config()
        {
//                Add(MemoryDiagnoser.Default);
//                Add(StatisticColumn.P95);

            Add(
                Job.LegacyJitX86
                    .WithLaunchCount(1)
                    .WithWarmupCount(3)
                    .WithTargetCount(5),
                Job.LegacyJitX64
                    .WithLaunchCount(1)
                    .WithWarmupCount(3)
                    .WithTargetCount(5),
                Job.RyuJitX64
                    .WithLaunchCount(1)
                    .WithWarmupCount(3)
                    .WithTargetCount(5));
        }
    }
}