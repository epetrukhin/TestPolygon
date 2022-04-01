using System;
using System.Buffers.Text;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using BenchmarkDotNet.Attributes;

namespace Benchmark
{
    // ReSharper disable once ClassCanBeSealed.Global
    [Config(typeof(Config))]
    public class Benchmark
    {
        private const int Count = 100_000;
        private static int[] values;

        private static ReadOnlyMemory<byte>[] serializedValues;

        private static JsonWriterOptions options;

        [GlobalSetup]
        public void Setup()
        {
            options = new JsonWriterOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Indented = false,
                SkipValidation = true
            };

            var buffer = new byte[1024 * 1024 * 1024];
            var currentDataIndex = 0;

            serializedValues = new ReadOnlyMemory<byte>[Count + 1011];

            values = new int[Count];
            var value = 1010;
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = value;
                Utf8Formatter.TryFormat(value / 1_000m, buffer.AsSpan(currentDataIndex), out var bytesWritten);

                serializedValues[value] = new ReadOnlyMemory<byte>(buffer, currentDataIndex, bytesWritten);

                value++;
                currentDataIndex += bytesWritten;
            }
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = Count)]
        public void Divide()
        {
            using (var writer = new Utf8JsonWriter(Stream.Null, options))
            {
                writer.WriteStartArray();

                foreach (var value in values)
                    writer.WriteNumberValue(value / 1_000m);

                writer.WriteEndArray();

                writer.Flush();
            }
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public void Preserialized()
        {
            var memories = serializedValues;

            using (var writer = new Utf8JsonWriter(Stream.Null, options))
            {
                writer.WriteStartArray();

                foreach (var value in values)
                {
                    if (value < memories.Length)
                    {
                        writer.WriteRawValue(memories[value].Span, true);
                    }
                    else
                    {
                        writer.WriteNumberValue(value / 1_000m);
                    }
                }

                writer.WriteEndArray();

                writer.Flush();
            }
        }
    }
}