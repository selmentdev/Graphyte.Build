using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Graphyte.Build.Generators
{
    public sealed class GeneratorProvider
    {
        public GeneratorFactory[] Factories { get; }

        public GeneratorProvider()
        {
            var providers = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(x => x.IsDefined(typeof(Core.TypesProviderAttribute)))
                .SelectMany(x => x.GetTypes())
                .Where(FilterType)
                .Select(x => Activator.CreateInstance(x))
                .Cast<GeneratorFactoryProvider>();

            this.Factories = providers
                .SelectMany(x => x.Provide())
                .ToArray();
        }

        private static bool FilterType(Type type)
        {
            return type.IsClass
                && !type.IsAbstract
                && type.IsSealed
                && type.IsDefined(typeof(GeneratorFactoryProviderAttribute))
                && type.IsSubclassOf(typeof(GeneratorFactory));
        }

        [Conditional("DEBUG")]
        public void Dump()
        {
            Trace.WriteLine("Generators:");
            Trace.Indent();

            foreach (var factory in this.Factories)
            {
                Trace.WriteLine(factory);
            }

            Trace.Unindent();
        }
    }
}
