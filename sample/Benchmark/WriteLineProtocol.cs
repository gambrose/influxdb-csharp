namespace Benchmark
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using InfluxDB.LineProtocol.Client;
    using InfluxDB.LineProtocol.Payload;

    public class WriteLineProtocol
    {
        private const int N = 100;

        private readonly (DateTime timestamp, double value)[] data;
        private readonly LineProtocolClient client;

        public WriteLineProtocol()
        {
            var random = new Random(755);
            var now = DateTime.UtcNow;
            data = Enumerable.Range(0, N).Select(i => (now.AddMilliseconds(random.Next(2000)), random.NextDouble())).ToArray();

            client = new EverythingIsAwesomeClient();
        }

        [Benchmark]
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
    }
}