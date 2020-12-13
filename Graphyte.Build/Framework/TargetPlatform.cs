using System;
using System.Diagnostics.CodeAnalysis;

namespace Graphyte.Build.Framework
{
    public readonly struct TargetPlatform
        : IEquatable<TargetPlatform>
    {
        public static readonly TargetPlatform Windows
            = new TargetPlatform("Windows");

        public static readonly TargetPlatform UniversalWindows
            = new TargetPlatform("UniversalWindows");

        public static readonly TargetPlatform Linux
            = new TargetPlatform("Linux");

        public static readonly TargetPlatform Android
            = new TargetPlatform("Android");

        public static readonly TargetPlatform MacOS
            = new TargetPlatform("MacOS");

        public static readonly TargetPlatform IOS
            = new TargetPlatform("IOS");

        private readonly string m_Value;

        private TargetPlatform(string value)
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

        public static TargetPlatform Create(string value)
        {
            return new TargetPlatform(value);
        }

        public override string ToString()
        {
            return this.m_Value ?? string.Empty;
        }

        public bool Equals([AllowNull] TargetPlatform other)
        {
            return this.Equals(other.m_Value);
        }

        private bool Equals(string value)
        {
            return string.Equals(this.m_Value, value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is TargetPlatform other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.m_Value.GetHashCode();
        }

        public static bool operator ==(TargetPlatform left, TargetPlatform right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TargetPlatform left, TargetPlatform right)
        {
            return !(left == right);
        }
    }
}
