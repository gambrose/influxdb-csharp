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

        private readonly (DateTime timestamp, double value)[] data;
        private readonly LineProtocolClient client;

        public WriteLineProtocol()
        {
            var random = new Random(755);
            var now = DateTime.UtcNow;
            data = Enumerable.Range(0, N).Select(i => (now.AddMilliseconds(random.Next(2000)), random.NextDouble())).ToArray();

            client = new EverythingIsAwesomeClient();
        }

        [Benchmark(Baseline = true)]
        public async Task<LineProtocolWriteResult> LineProtocolPoint()
        {
            var payload = new LineProtocolPayload();

            foreach (var point in data)
            {
                payload.Add(new LineProtocolPoint(
                    "example",
                    new Dictionary<string, object>
                    {
                        {"value", point.value}
                    },
                    null,
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
                payload.Add(new ExamplePoint(point.value, point.timestamp));
            }

            return await client.WriteAsync(payload);
        }
    }

    internal struct ExamplePoint : ILineProtocolPayload
    {
        public ExamplePoint(double value, DateTime timestamp)
        {
            Value = value;
            Timestamp = timestamp;
        }

        public double Value { get; }
        public DateTime Timestamp { get; }

        public void Format(LineProtocolWriter writer)
        {
            writer.Measurement("example").Field("value", Value).Timestamp(Timestamp);
        }
    }
}