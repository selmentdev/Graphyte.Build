using System;
using System.Diagnostics.CodeAnalysis;

namespace Graphyte.Build
{
    [Serializable]
    public readonly struct ToolsetType
        : IEquatable<ToolsetType>
    {
        public static ToolsetType Default = new ToolsetType("Default");
        public static ToolsetType Clang = new ToolsetType("Clang");
        public static ToolsetType ClangCL = new ToolsetType("ClangCL");
        public static ToolsetType GCC = new ToolsetType("GCC");
        public static ToolsetType MSVC = new ToolsetType("MSVC");

        private readonly string m_Value;

        internal ToolsetType(string value)
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

        public static ToolsetType Create(string value)
        {
            return new ToolsetType(value);
        }

        public override string ToString()
        {
            return this.m_Value ?? string.Empty;
        }

        public bool Equals([AllowNull] ToolsetType other)
        {
            return this.Equals(other.m_Value);
        }

        private bool Equals(string value)
        {
            return string.Equals(this.m_Value, value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is ToolsetType other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.m_Value.GetHashCode();
        }

        public static bool operator ==(ToolsetType left, ToolsetType right) => left.Equals(right);
        public static bool operator !=(ToolsetType left, ToolsetType right) => !(left == right);
    }
}
