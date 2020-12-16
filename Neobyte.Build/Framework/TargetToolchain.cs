using System;
using System.Diagnostics.CodeAnalysis;

namespace Neobyte.Build.Framework
{
    public readonly struct TargetToolchain
        : IEquatable<TargetToolchain>
    {
        public static readonly TargetToolchain MSVC
            = new TargetToolchain("MSVC");

        public static readonly TargetToolchain Clang
            = new TargetToolchain("Clang");

        public static readonly TargetToolchain ClangCL
            = new TargetToolchain("ClangCL");

        public static readonly TargetToolchain GCC
            = new TargetToolchain("GCC");

        public static readonly TargetToolchain ICC
            = new TargetToolchain("ICC");

        private readonly string m_Value;

        private TargetToolchain(string value)
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

        public static TargetToolchain Create(string value)
        {
            return new TargetToolchain(value);
        }

        public override string ToString()
        {
            return this.m_Value ?? string.Empty;
        }

        public bool Equals([AllowNull] TargetToolchain other)
        {
            return this.Equals(other.m_Value);
        }

        private bool Equals(string value)
        {
            return string.Equals(this.m_Value, value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is TargetToolchain other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.m_Value.GetHashCode();
        }

        public static bool operator ==(TargetToolchain left, TargetToolchain right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TargetToolchain left, TargetToolchain right)
        {
            return !(left == right);
        }
    }
}
