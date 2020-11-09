using System;
using System.Diagnostics.CodeAnalysis;

namespace Graphyte.Build
{
    public readonly struct PlatformType
        : IEquatable<PlatformType>
    {
        public static PlatformType Windows = new PlatformType("Windows");
        public static PlatformType UniversalWindows = new PlatformType("UniversalWindows");
        public static PlatformType Linux = new PlatformType("Linux");
        public static PlatformType Android = new PlatformType("Android");
        public static PlatformType MacOS = new PlatformType("MacOS");
        public static PlatformType IOS = new PlatformType("IOS");

        private readonly string m_Value;

        private PlatformType(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            this.m_Value = value;
        }

        public static PlatformType Create(string value)
        {
            return new PlatformType(value);
        }

        public override string ToString()
        {
            return this.m_Value ?? string.Empty;
        }

        public bool Equals([AllowNull] PlatformType other)
        {
            return this.Equals(other.m_Value);
        }

        private bool Equals(string value)
        {
            return string.Equals(this.m_Value, value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is PlatformType other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.m_Value.GetHashCode();
        }

        public static bool operator ==(PlatformType left, PlatformType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PlatformType left, PlatformType right)
        {
            return !(left == right);
        }
    }
}
