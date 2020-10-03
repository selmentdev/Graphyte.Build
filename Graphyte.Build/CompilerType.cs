using System;
using System.Diagnostics.CodeAnalysis;

namespace Graphyte.Build
{
    public readonly struct CompilerType
        : IEquatable<CompilerType>
    {
        public static CompilerType Clang = new CompilerType("Clang");
        public static CompilerType GCC = new CompilerType("GCC");
        public static CompilerType MSVC = new CompilerType("MSVC");

        private readonly string m_Value;

        private CompilerType(string value)
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

        public static CompilerType Create(string value)
        {
            return new CompilerType(value);
        }

        public override string ToString()
        {
            return this.m_Value ?? string.Empty;
        }

        public bool Equals([AllowNull] CompilerType other)
        {
            return this.Equals(other.m_Value);
        }

        private bool Equals(string value)
        {
            return string.Equals(this.m_Value, value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CompilerType other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.m_Value.GetHashCode();
        }

        public static bool operator ==(CompilerType left, CompilerType right) => left.Equals(right);

        public static bool operator !=(CompilerType left, CompilerType right) => !(left == right);
    }
}
