﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Alba.CsConsoleFormat;
using Functional;
using JetBrains.Annotations;

namespace ConsoleApp.Helpers;

[PublicAPI]
internal static class DumpExtensions
{
    private const int DefaultMaxItemsCount = 50;

    #region Type helpers
    private static readonly HashSet<Type> ScalarTypes =
    [
        typeof(object),
        typeof(string),
        typeof(decimal),
        typeof(DateTime),
        typeof(TimeSpan)
    ];
    private static bool IsScalar([NotNull] this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return
            type.IsPrimitive ||
            type.IsEnum ||
            ScalarTypes.Contains(type) ||
            IsGenericType(type, typeof(Nullable<>)) ||
            IsGenericType(type, typeof(Maybe<>)) ||
            type.BaseType != null && IsGenericType(type.BaseType, typeof(Either<,>));

        static bool IsGenericType(Type concreteType, Type genericType) =>
            concreteType.IsGenericType &&
            concreteType.GetGenericTypeDefinition() == genericType;
    }

    [NotNull]
    private static IEnumerable<(string name, Type type, Func<T, string> getter)> GetProperties<T>()
    {
        if (typeof(T).IsScalar())
            return new (string name, Type type, Func<T, string> getter)[]{ (typeof(T).Name, typeof(T), item => item.ConvertToString()) };

        return typeof(T)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(pi => pi.GetMethod != null)
            .Select<PropertyInfo, (string name, Type type, Func<T, string> getter)>(
                pi => (pi.Name, pi.PropertyType, item => GetPropertyValue(pi, item)));

        static string GetPropertyValue(PropertyInfo pi, object item)
        {
            try
            {
                return pi.GetValue(item).ConvertToString();
            }
            catch (Exception ex)
            {
                return $"<{ex.Message}>";
            }
        }
    }
    #endregion

    #region Dump helpers
    private static void DumpSequence<T>(
        [NotNull] IEnumerable<T> sequence, int maxItems, [CanBeNull] object name, [NotNull] Action<IEnumerable<T>> dumpItems)
    {
        var allItems = sequence.ToList();
        var totalCount = allItems.Count;

        GetHeaderText().WriteInfo();

        dumpItems(allItems.Take(maxItems));

        if (totalCount > maxItems)
            $"... {totalCount - maxItems} tail item(s) skipped.".WriteInfo();

        string GetHeaderText()
        {
            var nameString = name?.ToString();

            var result = string.IsNullOrWhiteSpace(nameString)
                ? string.Empty
                : $"{nameString}, ";

            result += $"{totalCount} item(s)";

            if (totalCount > maxItems)
                result += $", top {maxItems} item(s)";

            return result + ":";
        }
    }
    #endregion

    #region As string
    public static void DumpAsString([CanBeNull] this object obj, [CanBeNull] object name = null)
    {
        var nameString = name?.ToString();

        if (!string.IsNullOrWhiteSpace(nameString))
        {
            using (ConsoleHelpers.WithForegroundColor(ConsoleHelpers.InfoColor))
            {
                Console.Write($"{nameString}: ");
            }
        }

        obj.ConvertToString().WriteLine();
    }
    #endregion

    #region As list
    public static void DumpAsList<T>([CanBeNull] this IEnumerable<T> sequence, int maxItems, [CanBeNull] object name = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxItems);

        if (sequence == null)
        {
            DumpAsString(null, name);
            return;
        }

        DumpSequence(
            sequence,
            maxItems,
            name,
            items => string
                .Join(
                    Environment.NewLine,
                    items.Select(item => item.ConvertToString()))
                .WriteLine());
    }

    public static void DumpAsList<T>([CanBeNull] this IEnumerable<T> sequence, [CanBeNull] object name = null) =>
        sequence.DumpAsList(DefaultMaxItemsCount, name);
    #endregion

    #region As table
    private static readonly HashSet<Type> RightAlignTypes =
    [
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(short),
        typeof(ushort),
        typeof(byte),
        typeof(decimal),
        typeof(double),
        typeof(float)
    ];
    private static Align GetAligmentFor(Type type) =>
        RightAlignTypes.Contains(type) ? Align.Right : Align.Left;

    private static readonly LineThickness HeaderThickness = new(LineWidth.Single, LineWidth.Double);

    [NotNull]
    private static Document CreateDocumentCore<T>(
        [NotNull] IEnumerable<T> sequence,
        [NotNull] params (string name, Type type, Func<T, string> getter)[] props)
    {
        var grid = new Grid();

        foreach (var _ in props)
            grid.Columns.Add(new Column());

        foreach (var prop in props)
            grid.Children.Add(new Cell(prop.name) { Stroke = HeaderThickness, Align = GetAligmentFor(prop.type) });

        var cells =
            from item in sequence
            from prop in props
            select new Cell(prop.getter(item)) { Align = GetAligmentFor(prop.type) };

        foreach (var cell in cells)
            grid.Children.Add(cell);

        return new Document { Children = { grid } };
    }

    [NotNull]
    private static Document CreateDocument<TKey, TElement>([NotNull] IEnumerable<IGrouping<TKey, TElement>> sequence) =>
        CreateDocumentCore(
            sequence,
            ("Key", typeof(TKey), group => group.Key.ConvertToString()),
            ("Group", typeof(IGrouping<TKey, TElement>), group => group.ToList().ConvertToString()));

    [NotNull]
    private static Document CreateDocument<TKey, TValue>([NotNull] IEnumerable<KeyValuePair<TKey, TValue>> sequence) =>
        CreateDocumentCore(
            sequence,
            ("Key", typeof(TKey), kvp => kvp.Key.ConvertToString()),
            ("Value", typeof(TValue), kvp => kvp.Value.ConvertToString()));

    [NotNull]
    private static Document CreateDocument<T>([NotNull] IEnumerable<IEnumerable<T>> sequence) =>
        CreateDocumentCore(
            sequence,
            ("Item", typeof(IEnumerable<T>), items => items.ConvertToString()));

    [NotNull]
    private static Document CreateDocument<T>([NotNull] IEnumerable<T[]> sequence) =>
        CreateDocumentCore(
            sequence,
            ("Item", typeof(IEnumerable<T>), items => items.ConvertToString()));

    [NotNull]
    private static Document CreateDocument<T>([NotNull] IEnumerable<T> sequence) =>
        CreateDocumentCore(
            sequence,
            GetProperties<T>().ToArray());

    public static void DumpAsTable<T>([CanBeNull] this IEnumerable<T> sequence, int maxItems, [CanBeNull] object name = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxItems);

        if (sequence == null)
        {
            DumpAsString(null, name);
            return;
        }

        DumpSequence(
            sequence,
            maxItems,
            name,
            items => ConsoleRenderer.RenderDocument(CreateDocument((dynamic)items)));
    }

    public static void DumpAsTable<T>([CanBeNull] this IEnumerable<T> sequence, [CanBeNull] object name = null) =>
        sequence.DumpAsTable(DefaultMaxItemsCount, name);
    #endregion

    #region Dump
    private static void DumpCore<T>([NotNull] IEnumerable<T> sequence, [CanBeNull] object name)
    {
        if (typeof(T).IsScalar())
            sequence.DumpAsList(name);
        else
            sequence.DumpAsTable(name);
    }

    private static void DumpCore([NotNull] string str, [CanBeNull] object name) =>
        str.DumpAsString(name);

    private static void DumpCore([NotNull] object obj, [CanBeNull] object name) =>
        obj.DumpAsString(name);

    public static void Dump([CanBeNull] this object obj, [CanBeNull] object name = null)
    {
        if (obj == null)
        {
            DumpAsString(null, name);
            return;
        }

        DumpCore((dynamic)obj, name);
    }
    #endregion
}