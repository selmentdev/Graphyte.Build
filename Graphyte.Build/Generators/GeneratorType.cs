using System;
using System.Diagnostics.CodeAnalysis;

namespace Graphyte.Build.Generators
{
    public readonly struct GeneratorType
        : IEquatable<GeneratorType>
    {
        public static GeneratorType MSBuild = new GeneratorType("MSBuild");
        public static GeneratorType FastBuild = new GeneratorType("FastBuild");
        public static GeneratorType CMake = new GeneratorType("CMake");
        public static GeneratorType MakeFile = new GeneratorType("MakeFile");

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
            return new GeneratorType(value);
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
