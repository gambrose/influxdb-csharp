using System;

namespace InfluxDB.LineProtocol
{
    public struct LineProtocolName : IEquatable<LineProtocolName>
    {
        internal readonly string Escaped;

        public LineProtocolName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name));
            }

            this.Escaped = EscapeName(name);
        }

        public override string ToString()
        {
            return UnescapeName(this.Escaped);
        }

        private static string EscapeName(string name)
        {
            return name
                .Replace("=", "\\=")
                .Replace(" ", "\\ ")
                .Replace(",", "\\,");
        }

        private static string UnescapeName(string name)
        {
            return name
                .Replace("\\=", "=")
                .Replace("\\ ", " ")
                .Replace("\\,", ",");
        }

        public bool Equals(LineProtocolName other)
        {
            return string.Equals(Escaped, other.Escaped);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is LineProtocolName && Equals((LineProtocolName)obj);
        }

        public override int GetHashCode()
        {
            return Escaped?.GetHashCode() ?? 0;
        }

        public static bool operator ==(LineProtocolName left, LineProtocolName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LineProtocolName left, LineProtocolName right)
        {
            return !left.Equals(right);
        }
    }
}