using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using JetBrains.Annotations;

namespace BenchmarkCore
{
    // ReSharper disable once ClassCanBeSealed.Global
    [Config(typeof(Config))]
    public class Benchmark
    {
        // ReSharper disable once EmptyConstructor
        public Benchmark()
        {
            _team1 = new int[Size];
            _team2 = new int[Size];
            _results = new string[Size];

            var rnd = new Random(42);

            for (var i = 0; i < Size; i++)
            {
                _team1[i] = rnd.Next(0, 100);
                _team2[i] = rnd.Next(0, 100);
            }

            var scoresToCache =
                from team1 in Enumerable.Range(0, 100)
                from team2 in Enumerable.Range(0, 100)
                select (team1, team2);

            _dictCache = scoresToCache.ToDictionary(score => score, score => ScoreToString(score.team1, score.team2));

            _arrayCache = new string[100 * 100];
            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    _arrayCache[ScoreToIndex(i, j)] = ScoreToString(i, j);
                }
            }
        }

        private readonly int[] _team1;
        private readonly int[] _team2;

        private readonly Dictionary<(int, int), string> _dictCache;
        private readonly string[] _arrayCache;

        private readonly string[] _results;

        private const int Size = 100_000;

        private static string ScoreToString(int team1, int team2) =>
            team1.ToString(CultureInfo.InvariantCulture) +
            ":" +
            team2.ToString(CultureInfo.InvariantCulture);

        private static int ScoreToIndex(int team1, int team2) =>
            team1 * 100 + team2;

        [Benchmark(Baseline = true, OperationsPerInvoke = Size)]
        public void Calc()
        {
            for (var i = 0; i < Size; i++)
            {
                _results[i] = ScoreToString(_team1[i], _team2[i]);
            }
        }

        [Benchmark( OperationsPerInvoke = Size)]
        public void DictCache()
        {
            for (var i = 0; i < Size; i++)
            {
                _results[i] = _dictCache[(_team1[i], _team2[i])];
            }
        }

        [Benchmark( OperationsPerInvoke = Size)]
        public void ArrayCache()
        {
            for (var i = 0; i < Size; i++)
            {
                _results[i] = _arrayCache[ScoreToIndex(_team1[i], _team2[i])];
            }
        }
    }
}