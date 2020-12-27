using System;
using System.Diagnostics.CodeAnalysis;

namespace Neobyte.Build.Generators
{
    public readonly struct GeneratorType
        : IEquatable<GeneratorType>
    {
        public static readonly GeneratorType MSBuild = new("MSBuild");
        public static readonly GeneratorType FastBuild = new("FastBuild");
        public static readonly GeneratorType CMake = new("CMake");
        public static readonly GeneratorType MakeFile = new("MakeFile");

        private readonly string m_Value;

        private GeneratorType(string value)
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

        public static GeneratorType Create(string value)
        {
            return new(value);
        }

        public override string ToString()
        {
            return this.m_Value ?? string.Empty;
        }

        public bool Equals([AllowNull] GeneratorType other)
        {
            return this.Equals(other.m_Value);
        }

        private bool Equals(string value)
        {
            return string.Equals(this.m_Value, value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is GeneratorType other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.m_Value.GetHashCode();
        }

        public static bool operator ==(GeneratorType left, GeneratorType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GeneratorType left, GeneratorType right)
        {
            return !(left == right);
        }
    }
}
