using System;
using System.Diagnostics.CodeAnalysis;

namespace Neobyte.Build.Framework
{
    public readonly struct ModuleRulesFactory
    {
        public readonly Type Type;

        public ModuleRulesFactory(Type type)
        {
            this.Type = type;
        }

        public ModuleRules Create(TargetRules target)
        {
            return (ModuleRules)Activator.CreateInstance(this.Type, target)!;
        }

        public override string ToString()
        {
            return this.Type.ToString();
        }

        public bool Equals([AllowNull] ModuleRulesFactory other)
        {
            return this.Type.Equals(other.Type);
        }

        public override bool Equals(object? obj)
        {
            return obj is ModuleRulesFactory other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode();
        }

        public static bool operator ==(ModuleRulesFactory left, ModuleRulesFactory right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ModuleRulesFactory left, ModuleRulesFactory right)
        {
            return !(left == right);
        }
    }
}
