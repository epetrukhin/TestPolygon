using System;
using System.Text;
using System.Threading;
using ConsoleApp.Helpers;
using JetBrains.Annotations;

namespace ConsoleApp
{
    internal static partial class Program
    {
        [STAThread]
        private static void Main()
        {
            Console.WindowWidth = Console.LargestWindowWidth - 20;
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

            WaitKey("Press any key to close...");
        }

        private static void CurrentDomainOnUnhandledException(object _, [NotNull] UnhandledExceptionEventArgs args) =>
            DumpException((Exception)args.ExceptionObject);

        [PublicAPI]
        private static void DumpException(Exception ex)
        {
            Console.WriteLine();
            using (ForegroundColor(ConsoleHelpers.ErrorColor))
            {
                SeparatorLine();
                ex.ToString().WriteLine();
                SeparatorLine();
            }
            Console.WriteLine();
        }

        [PublicAPI]
        private static void BlankLine() => Console.WriteLine();

        [PublicAPI]
        public static void ClearConsole() => Console.Clear();

        [PublicAPI]
        private static void SeparatorLine() => Console.Write(new string('=', Console.WindowWidth));

        [PublicAPI]
        private static void WaitKey(string prompt = null)
        {
            if (!string.IsNullOrWhiteSpace(prompt))
                prompt.WriteWarning();

            Console.ReadKey();
        }

        [PublicAPI]
        private static void Sleep(int milliseconds) => Thread.Sleep(milliseconds);

        [PublicAPI]
        private static void Sleep(TimeSpan interval) => Thread.Sleep(interval);

        [PublicAPI, NotNull]
        public static IDisposable ForegroundColor(ConsoleColor color) => ConsoleHelpers.WithForegroundColor(color);
    }
}