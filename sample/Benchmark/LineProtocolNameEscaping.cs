namespace Benchmark
{
    using System;
    using BenchmarkDotNet.Attributes;
    using InfluxDB.LineProtocol;

    public class LineProtocolNameEscaping
    {
        private const int N = 500;

        private const string MEASUREMENT_NAME = "example";
        private const string TAG_NAME = "colour";
        private const string FIELD_NAME = "value";

        private static readonly LineProtocolName MeasurementName = new LineProtocolName(MEASUREMENT_NAME);
        private static readonly LineProtocolName TagName = new LineProtocolName(TAG_NAME);
        private static readonly LineProtocolName FieldName = new LineProtocolName(FIELD_NAME);

        private readonly DateTime timestamp = DateTime.Now;

        [Benchmark(Baseline = true)]
        public void Strings()
        {
            var writer = new LineProtocolWriter();

            for (int i = 0; i < N; i++)
            {
                writer.Measurement(MEASUREMENT_NAME).Tag(TAG_NAME, "blue").Field(FIELD_NAME, 10).Timestamp(timestamp);
            }
        }

        [Benchmark]
        public void PreEscaped()
        {
            var writer = new LineProtocolWriter();

            for (int i = 0; i < N; i++)
            {
                writer.Measurement(MeasurementName).Tag(TagName, "blue").Field(FieldName, 10).Timestamp(timestamp);
            }
        }
    }
}