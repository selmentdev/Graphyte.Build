using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Neobyte.Build.Framework
{
    public readonly struct TargetRuleTypeDescriptor
    {
        public readonly Type Type;
        public readonly TargetFlavor[] Flavors;

        public TargetRuleTypeDescriptor(Type type)
        {
            this.Type = type;
            this.Flavors = GetTargetFlavors(type);
        }

        public TargetRules Create(TargetDescriptor descriptor, TargetContext context)
        {
            return Activator.CreateInstance(this.Type, descriptor, context) as TargetRules;
        }

        private static TargetFlavor[] GetTargetFlavors(Type type)
        {
            return type
                .GetCustomAttributes<TargetRulesFlavorAttribute>()
                .Select(x => x.Flavor)
                .ToArray();
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
                .Select(x => new TargetRuleTypeDescriptor(x))
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

        public TargetRuleTypeDescriptor[] Targets { get; }

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
