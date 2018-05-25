using System;
using System.Reactive.Disposables;
using JetBrains.Annotations;

namespace ConsoleApp.Helpers
{
    [PublicAPI]
    internal static class ConsoleHelpers
    {
        public const ConsoleColor ErrorColor   = ConsoleColor.Red;
        public const ConsoleColor WarningColor = ConsoleColor.Yellow;
        public const ConsoleColor InfoColor    = ConsoleColor.DarkGreen;

        public static void WriteLine([CanBeNull] this string text) => Console.WriteLine(text ?? "<null>");

        public static void WriteError([CanBeNull] this string text)   => text.WriteWithColor(ErrorColor);
        public static void WriteWarning([CanBeNull] this string text) => text.WriteWithColor(WarningColor);
        public static void WriteInfo([CanBeNull] this string text)    => text.WriteWithColor(InfoColor);

        public static void WriteWithColor([CanBeNull] this string text, ConsoleColor color)
        {
            using (WithForegroundColor(color))
                text.WriteLine();
        }

        [NotNull]
        public static IDisposable WithForegroundColor(ConsoleColor color)
        {
            var currentColor = Console.ForegroundColor;

            if (currentColor == color)
                return Disposable.Empty;

            Console.ForegroundColor = color;

            return Disposable.Create(() => Console.ForegroundColor = currentColor);
        }
    }
}