using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace Benchmark
{
    public sealed class Config : ManualConfig
    {
        public Config()
        {
            AddDiagnoser(MemoryDiagnoser.Default);
            AddColumn(StatisticColumn.P95);
            AddColumn(StatisticColumn.OperationsPerSecond);

            AddJob(
                Job.Default.WithRuntime(CoreRuntime.Core60)
                    .WithLaunchCount(1)
                    .WithWarmupCount(3)
                    .WithIterationCount(10));

            // AddDiagnoser(MemoryDiagnoser.Default);
            // AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig()));
            // AddColumn(StatisticColumn.P95);
            // AddColumn(StatisticColumn.OperationsPerSecond);
            //
            // AddJob(
            //     Job.Default.WithRuntime(CoreRuntime.Core50)
            //         .WithLaunchCount(2)
            //         .WithWarmupCount(3)
            //         .WithIterationCount(10));
        }
    }
}