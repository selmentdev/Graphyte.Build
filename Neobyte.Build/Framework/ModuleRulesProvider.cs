using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Neobyte.Build.Framework
{
    public readonly struct ModuleRulesMetadata
    {
        public readonly Type Type;

        public ModuleRulesMetadata(Type type)
        {
            this.Type = type;
        }

        public ModuleRules Create(TargetRules target)
        {
            return Activator.CreateInstance(this.Type, target) as ModuleRules;
        }

        public override string ToString()
        {
            return this.Type.ToString();
        }

        public bool Equals([AllowNull] ModuleRulesMetadata other)
        {
            return this.Type.Equals(other.Type);
        }

        public override bool Equals(object obj)
        {
            return obj is ModuleRulesMetadata other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode();
        }

        public static bool operator ==(ModuleRulesMetadata left, ModuleRulesMetadata right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ModuleRulesMetadata left, ModuleRulesMetadata right)
        {
            return !(left == right);
        }
    }
}

namespace Neobyte.Build.Framework
{
    public sealed class ModuleRulesProvider
    {
        public ModuleRulesProvider()
        {
            this.Modules = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(x => x.IsDefined(typeof(Core.TypesProviderAttribute)))
                .SelectMany(x => x.GetTypes())
                .Where(Filter)
                .Select(x => new ModuleRulesMetadata(x))
                .ToArray();
        }

        private static bool Filter(Type x)
        {
            return x.IsSubclassOf(typeof(ModuleRules))
                && x.IsClass
                && !x.IsAbstract
                && x.IsVisible
                && x.IsDefined(typeof(ModuleRulesAttribute));
        }

        public ModuleRulesMetadata[] Modules { get; }

        [Conditional("DEBUG")]
        public void Dump()
        {
            Trace.WriteLine("Modules");
            Trace.Indent();

            foreach (var module in this.Modules)
            {
                Trace.WriteLine(module);
            }

            Trace.Unindent();
        }
    }
}
