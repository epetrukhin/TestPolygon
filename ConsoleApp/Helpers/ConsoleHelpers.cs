using System;
using System.Reactive.Disposables;
using JetBrains.Annotations;

namespace ConsoleApp.Helpers
{
    [PublicAPI]
    internal static class ConsoleHelpers
    {
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