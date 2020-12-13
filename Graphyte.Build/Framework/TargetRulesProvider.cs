using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Graphyte.Build.Framework
{
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

        public Type[] Targets { get; }

        public static TargetRules Create(Type type, TargetDescriptor descriptor, TargetContext context)
        {
            return Activator.CreateInstance(type, descriptor, context) as TargetRules;
        }

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
