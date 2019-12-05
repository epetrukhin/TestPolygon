using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace BenchmarkCore
{
    public sealed class Config : ManualConfig
    {
        public Config()
        {
            Add(MemoryDiagnoser.Default);
            Add(StatisticColumn.P95);

            Add(
                Job.Default.With(CoreRuntime.Core30)
                    .WithLaunchCount(1)
                    .WithWarmupCount(1)
                    .WithIterationCount(3),
                Job.Default.With(ClrRuntime.Net48)
                    .WithLaunchCount(1)
                    .WithWarmupCount(1)
                    .WithIterationCount(3));
        }
    }
}