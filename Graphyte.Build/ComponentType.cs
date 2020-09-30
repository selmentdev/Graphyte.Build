using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Graphyte.Build
{
    public readonly struct ComponentType
        : IEquatable<ComponentType>
    {
        public static ComponentType Application = new ComponentType("Application");
        public static ComponentType UnitTest = new ComponentType("UnitTest");
        public static ComponentType Module = new ComponentType("Module");

        private readonly string m_Value;

        private ComponentType(string value)
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

        public static ComponentType Create(string value)
        {
            return new ComponentType(value);
        }

        public override string ToString()
        {
            return this.m_Value ?? string.Empty;
        }

        public bool Equals([AllowNull] ComponentType other)
        {
            return this.Equals(other.m_Value);
        }

        private bool Equals(string value)
        {
            return string.Equals(this.m_Value, value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is ComponentType other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.m_Value.GetHashCode();
        }

        public static bool operator ==(ComponentType left, ComponentType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ComponentType left, ComponentType right)
        {
            return !(left == right);
        }
    }
}
