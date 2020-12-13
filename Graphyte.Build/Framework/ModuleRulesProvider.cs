using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Graphyte.Build.Framework
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

        public Type[] Modules { get; }

        public static ModuleRules Create(Type type, TargetRules targetRules)
        {
            return Activator.CreateInstance(type, targetRules) as ModuleRules;
        }

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
