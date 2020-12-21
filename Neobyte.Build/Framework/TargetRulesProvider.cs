using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Neobyte.Build.Framework
{
    public readonly struct TargetRulesMetadata
    {
        public readonly Type Type;

        public TargetRulesMetadata(Type type)
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

        public bool Equals([AllowNull] TargetRulesMetadata other)
        {
            return this.Type.Equals(other.Type);
        }

        public override bool Equals(object obj)
        {
            return obj is TargetRulesMetadata other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode();
        }

        public static bool operator ==(TargetRulesMetadata left, TargetRulesMetadata right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TargetRulesMetadata left, TargetRulesMetadata right)
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

    public sealed class TargetRulesProvider
    {
        public TargetRulesProvider()
        {
            this.Targets = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(x => x.IsDefined(typeof(Core.TypesProviderAttribute)))
                .SelectMany(x => x.GetTypes())
                .Where(Filter)
                .Select(x => new TargetRulesMetadata(x))
                .ToArray();
        }

        private static bool Filter(Type type)
        {
            return type.IsSubclassOf(typeof(TargetRules))
                && type.IsClass
                && !type.IsAbstract
                && type.IsVisible
                && type.IsDefined(typeof(TargetRulesAttribute));
        }

        public TargetRulesMetadata[] Targets { get; }

        [Conditional("DEBUG")]
        public void Dump()
        {
            Trace.WriteLine("Targets");
            Trace.Indent();

            foreach (var target in this.Targets)
            {
                Trace.WriteLine(target);
            }

            Trace.Unindent();
        }
    }
}
