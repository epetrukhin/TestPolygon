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
            AddDiagnoser(MemoryDiagnoser.Default);
            AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig()));
            AddColumn(StatisticColumn.P95);

            AddJob(
                Job.Default.WithRuntime(CoreRuntime.Core31)
                    .WithLaunchCount(1)
                    .WithWarmupCount(2)
                    .WithIterationCount(5),
                Job.Default.WithRuntime(CoreRuntime.Core50)
                    .WithLaunchCount(1)
                    .WithWarmupCount(2)
                    .WithIterationCount(5));
        }
    }
}