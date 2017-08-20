﻿using System;
using System.Globalization;
using System.IO;

namespace InfluxDB.LineProtocol
{
    public class LineProtocolWriter
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly TextWriter textWriter;

        public LineProtocolWriterPosition Position { get; private set; } = LineProtocolWriterPosition.NothingWritten;

        public LineProtocolWriter()
        {
            this.textWriter = new StringWriter();
        }

        public LineProtocolWriter Measurement(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name));
            }

            switch (Position)
            {
                case LineProtocolWriterPosition.NothingWritten:
                    break;
                case LineProtocolWriterPosition.FieldWritten:
                case LineProtocolWriterPosition.TimestampWritten:
                    textWriter.Write("\n");
                    break;
                default:
                    throw InvalidPositionException($"Cannot write new measurement \"{name}\" as no field written for current line.");
            }

            foreach (char c in name)
            {
                switch (c)
                {
                    case ' ':
                        textWriter.Write("\\ ");
                        break;
                    case ',':
                        textWriter.Write("\\,");
                        break;
                    default:
                        textWriter.Write(c);
                        break;
                }
            }

            Position = LineProtocolWriterPosition.MeasurementWritten;

            return this;
        }

        public LineProtocolWriter Tag(string name, string value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            switch (Position)
            {
                case LineProtocolWriterPosition.MeasurementWritten:
                case LineProtocolWriterPosition.TagWritten:
                    textWriter.Write(",");
                    break;
                case LineProtocolWriterPosition.NothingWritten:
                    throw InvalidPositionException($"Cannot write tag \"{name}\" as no measurement name written.");
                default:
                    throw InvalidPositionException($"Cannot write tag \"{name}\" as field(s) already written for current line.");
            }

            WriteEscapedName(name);
            textWriter.Write('=');
            WriteEscapedName(value);

            Position = LineProtocolWriterPosition.TagWritten;

            return this;
        }

        public LineProtocolWriter Field(string name, float value)
        {
            WriteFieldKey(name);
            textWriter.Write('=');
            textWriter.Write(value.ToString(CultureInfo.InvariantCulture));

            Position = LineProtocolWriterPosition.FieldWritten;

            return this;
        }

        public LineProtocolWriter Field(string name, double value)
        {
            WriteFieldKey(name);
            textWriter.Write('=');
            textWriter.Write(value.ToString(CultureInfo.InvariantCulture));

            Position = LineProtocolWriterPosition.FieldWritten;

            return this;
        }

        public LineProtocolWriter Field(string name, decimal value)
        {
            WriteFieldKey(name);
            textWriter.Write('=');
            textWriter.Write(value.ToString(CultureInfo.InvariantCulture));

            Position = LineProtocolWriterPosition.FieldWritten;

            return this;
        }

        public LineProtocolWriter Field(string name, long value)
        {
            WriteFieldKey(name);
            textWriter.Write('=');
            textWriter.Write(value.ToString(CultureInfo.InvariantCulture));
            textWriter.Write('i');

            Position = LineProtocolWriterPosition.FieldWritten;

            return this;
        }

        public LineProtocolWriter Field(string name, string value)
        {
            WriteFieldKey(name);
            textWriter.Write('=');
            textWriter.Write('"');
            textWriter.Write(value.Replace("\"", "\\\""));
            textWriter.Write('"');

            Position = LineProtocolWriterPosition.FieldWritten;

            return this;
        }

        public LineProtocolWriter Field(string name, bool value)
        {
            WriteFieldKey(name);
            textWriter.Write('=');
            textWriter.Write(value ? 't' : 'f');

            Position = LineProtocolWriterPosition.FieldWritten;

            return this;
        }

        public void Timestamp(long value)
        {
            switch (Position)
            {
                case LineProtocolWriterPosition.FieldWritten:
                    textWriter.Write(" ");
                    break;
                case LineProtocolWriterPosition.NothingWritten:
                    throw InvalidPositionException("Cannot write timestamp as no measurement name written.");
                default:
                    throw InvalidPositionException("Cannot write timestamp as no field written for current measurement.");
            }

            textWriter.Write(value.ToString(CultureInfo.InvariantCulture));

            Position = LineProtocolWriterPosition.TimestampWritten;
        }

        public void Timestamp(TimeSpan value)
        {
            Timestamp(value.Ticks * 100L);
        }

        public void Timestamp(DateTimeOffset value)
        {
            Timestamp(value.UtcDateTime);
        }

        public void Timestamp(DateTime value)
        {
            if (value != null && value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Timestamps must be specified as UTC", nameof(value));
            }

            Timestamp(value - UnixEpoch);
        }

        public override string ToString()
        {
            return textWriter.ToString();
        }

        private void WriteFieldKey(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            switch (Position)
            {
                case LineProtocolWriterPosition.MeasurementWritten:
                case LineProtocolWriterPosition.TagWritten:
                    textWriter.Write(" ");
                    break;
                case LineProtocolWriterPosition.FieldWritten:
                    textWriter.Write(",");
                    break;
                default:
                    throw InvalidPositionException($"Cannot write field \"{name}\" as no measurement name written.");
            }

            WriteEscapedName(name);
        }

        private void WriteEscapedName(string name)
        {
            foreach (char c in name)
            {
                switch (c)
                {
                    case ' ':
                        textWriter.Write("\\ ");
                        break;
                    case ',':
                        textWriter.Write("\\,");
                        break;
                    case '=':
                        textWriter.Write("\\=");
                        break;
                    default:
                        textWriter.Write(c);
                        break;
                }
            }
        }

        private InvalidOperationException InvalidPositionException(string message)
        {
            // We don't need an custom exceptions as there should be no need for a dev to catch this condition. They are not using the api right so how can the write code to recover.
            // We can make the current writer position available for better diagnostics then logged.
            return new InvalidOperationException(message)
            {
                Data =
                {
                    { "Position", Position }
                }
            };
        }
    }
}
