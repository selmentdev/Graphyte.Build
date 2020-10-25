using Graphyte.Build.Toolchains;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Graphyte.Build.Generators
{
    public sealed class GeneratorsProvider
    {
        private static Type[] Discover()
        {
            var baseType = typeof(BaseGenerator);

            var result = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(baseType) && x.IsClass && !x.IsAbstract && x.IsVisible && x.IsSealed)
                .ToArray();

#if DEBUG
            foreach (var generator in result)
            {
                Debug.Assert(generator.Name.EndsWith("Generator"));
            }
#endif

            return result;
        }

        public Type[] Generators { get; } = GeneratorsProvider.Discover();

        public BaseGenerator Create(GeneratorType generator)
        {
            var name = $@"{generator}Generator";
            var type = this.Generators.FirstOrDefault(x => x.Name == name);

            if (type != null)
            {
                return (BaseGenerator)Activator.CreateInstance(type);
            }

            return null;
        }
    }
}
