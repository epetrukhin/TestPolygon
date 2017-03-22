﻿using System;
using System.Text;
using System.Threading;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using ConsoleApp.Helpers;
using JetBrains.Annotations;

namespace ConsoleApp
{
    internal static partial class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            try
            {
                ProgramCode();
            }
            catch (Exception ex)
            {
                DumpException(ex);
            }
            finally
            {
                AppDomain.CurrentDomain.UnhandledException -= CurrentDomainOnUnhandledException;
            }

	        using (ForegroundColor(ConsoleColor.Yellow))
	        {
		        Console.Write("Press any key to close...");
	        }

            WaitKey();
        }

        private static void CurrentDomainOnUnhandledException(object _, [NotNull] UnhandledExceptionEventArgs args) =>
			DumpException((Exception)args.ExceptionObject);

        [PublicAPI]
        private static void DumpException(Exception ex)
        {
            Console.WriteLine();
	        using (ForegroundColor(ConsoleColor.Red))
	        {
		        SeparatorLine();
		        Console.WriteLine(ex);
		        SeparatorLine();
	        }
            Console.WriteLine();
        }

        [PublicAPI]
        private static void BlankLine() => Console.WriteLine();

        [PublicAPI]
		private static void SeparatorLine() => Console.Write(new string('=', Console.WindowWidth));

        [PublicAPI]
        private static void WaitKey(string prompt = null)
        {
            if (!string.IsNullOrWhiteSpace(prompt))
                Console.WriteLine(prompt);

            Console.ReadKey();
        }

        [PublicAPI]
        private static void Sleep(int milliseconds) => Thread.Sleep(milliseconds);

		[PublicAPI]
		private static void Sleep(TimeSpan interval) => Thread.Sleep(interval);

		[PublicAPI, NotNull]
		public static IDisposable ForegroundColor(ConsoleColor color) => ConsoleHelpers.WithForegroundColor(color);

		[PublicAPI]
		private static void RunBenchmark() =>
			BenchmarkRunner
				.Run<Benchmark>(
					ManualConfig
						.Create(DefaultConfig.Instance)
						.With(
							Job.Default
								.WithLaunchCount(1)
								.WithWarmupCount(3)
								.WithTargetCount(5)))
				.ToString()
				.WriteLine();
	}
}