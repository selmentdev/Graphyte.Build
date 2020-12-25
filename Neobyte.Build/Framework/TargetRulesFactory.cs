using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Neobyte.Build.Framework
{
    public readonly struct TargetRulesFactory
    {
        public readonly Type Type;

        public TargetRulesFactory(Type type)
        {
            this.Type = type;
        }

        public TargetRules Create(TargetDescriptor descriptor, TargetContext context)
        {
            return Activator.CreateInstance(this.Type, descriptor, context) as TargetRules;
        }

        public override string ToString()
        {
            return this.Type.ToString();
        }

        public bool Equals([AllowNull] TargetRulesFactory other)
        {
            return this.Type.Equals(other.Type);
        }

        public override bool Equals(object obj)
        {
            return obj is TargetRulesFactory other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode();
        }

        public static bool operator ==(TargetRulesFactory left, TargetRulesFactory right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TargetRulesFactory left, TargetRulesFactory right)
        {
            return !(left == right);
        }

        public IEnumerable<TargetFlavor> GetSupportedFlavors()
        {
            return this.Type
                .GetCustomAttributes<TargetRulesFlavorAttribute>()
                .Select(x => x.Flavor);
        }
    }
}
