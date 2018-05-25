using System;
using JetBrains.Annotations;

namespace ConsoleApp.Helpers
{
    [PublicAPI]
    internal static class MeasuringUnitsExtensions
    {
        public static TimeSpan Ticks(this int value)        => TimeSpan.FromTicks(value);
        public static TimeSpan Milliseconds(this int value) => TimeSpan.FromMilliseconds(value);
        public static TimeSpan Seconds(this int value)      => TimeSpan.FromSeconds(value);
        public static TimeSpan Minutes(this int value)      => TimeSpan.FromMinutes(value);
        public static TimeSpan Hours(this int value)        => TimeSpan.FromHours(value);
        public static TimeSpan Days(this int value)         => TimeSpan.FromDays(value);

        private const int Kilo = 1_024;
        public static int Kb(this int value) => value * Kilo;
        public static int Mb(this int value) => value.Kb() * Kilo;
        public static int Gb(this int value) => value.Mb() * Kilo;
    }
}