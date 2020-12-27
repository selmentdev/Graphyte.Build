using System;
using System.Linq;
using System.Reflection;

namespace Neobyte.Build.Core
{
    public static class TypesProvider
    {
        static TypesProvider()
        {
            Assemblies = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(x => x.IsDefined(typeof(TypesProviderAttribute)))
                .ToArray();
        }

        public static readonly Assembly[] Assemblies;
    }
}
