namespace Benchmark
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using InfluxDB.LineProtocol;
    using InfluxDB.LineProtocol.Client;
    using InfluxDB.LineProtocol.Payload;

    public class WriteLineProtocol
    {
        private const int N = 500;

        private const string MEASUREMENT_NAME = "example";
        private const string TAG_NAME = "colour";
        private const string FIELD_NAME = "value";

        private static readonly string[] Colours = { "red", "blue", "green" };

        private readonly (DateTime timestamp, string colour, double value)[] data;
        private readonly LineProtocolClient client;

        public WriteLineProtocol()
        {
            var random = new Random(755);
            var now = DateTime.UtcNow;
            data = Enumerable.Range(0, N)
                .Select(i => (now.AddMilliseconds(random.Next(2000)), Colours[random.Next(Colours.Length)], random
                    .NextDouble())).ToArray();

            client = new EverythingIsAwesomeClient();
        }

        [Benchmark(Baseline = true)]
        public async Task<LineProtocolWriteResult> LineProtocolPoint()
        {
            var payload = new LineProtocolPayload();

            foreach (var point in data)
            {
                payload.Add(new LineProtocolPoint(
                    MEASUREMENT_NAME,
                    new Dictionary<string, object>
                    {
                        {FIELD_NAME, point.value}
                    },
                    new Dictionary<string, string>
                    {
                        {TAG_NAME, point.colour}
                    },
                    point.timestamp
                ));
            }

            return await client.WriteAsync(payload);
        }

        [Benchmark]
        public async Task<LineProtocolWriteResult> CustomStruct()
        {
            var payload = new List<ExamplePoint>(data.Length);

            foreach (var point in data)
            {
                payload.Add(new ExamplePoint(point.value, point.colour, point.timestamp));
            }

            return await client.WriteAsync(payload);
        }

        private struct ExamplePoint : ILineProtocolPayload
        {
            private static readonly LineProtocolName MeasurementName = new LineProtocolName(MEASUREMENT_NAME);
            private static readonly LineProtocolName ColourTagName = new LineProtocolName(TAG_NAME);
            private static readonly LineProtocolName ValueFieldName = new LineProtocolName(FIELD_NAME);

            public ExamplePoint(double value, string colour, DateTime timestamp)
            {
                Colour = colour;
                Value = value;
                Timestamp = timestamp;
            }

            public string Colour { get; }
            public double Value { get; }
            public DateTime Timestamp { get; }

            public void Format(LineProtocolWriter writer)
            {
                writer.Measurement(MeasurementName).Tag(ColourTagName, Colour).Field(ValueFieldName, Value)
                    .Timestamp(Timestamp);
            }
        }
    }
}