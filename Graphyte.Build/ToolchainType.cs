using System;
using System.Diagnostics.CodeAnalysis;

namespace Graphyte.Build
{
    public readonly struct ToolchainType
        : IEquatable<ToolchainType>
    {
        public static ToolchainType MSVC = new ToolchainType("MSVC");
        public static ToolchainType Clang = new ToolchainType("Clang");
        public static ToolchainType ClangCL = new ToolchainType("ClangCL");
        public static ToolchainType GCC = new ToolchainType("GCC");
        public static ToolchainType ICC = new ToolchainType("ICC");

        private readonly string m_Value;

        private ToolchainType(string value)
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

        public static ToolchainType Create(string value)
        {
            return new ToolchainType(value);
        }

        public override string ToString()
        {
            return this.m_Value ?? string.Empty;
        }

        public bool Equals([AllowNull] ToolchainType other)
        {
            return this.Equals(other.m_Value);
        }

        private bool Equals(string value)
        {
            return string.Equals(this.m_Value, value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is ToolchainType other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.m_Value.GetHashCode();
        }

        public static bool operator ==(ToolchainType left, ToolchainType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ToolchainType left, ToolchainType right)
        {
            return !(left == right);
        }
    }
}
