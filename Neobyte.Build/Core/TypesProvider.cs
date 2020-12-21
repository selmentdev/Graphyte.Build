using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neobyte.Build.Core
{
    public static class TypesProvider
    {
        static TypesProvider()
        {
            TypesProvider.Assemblies = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(x => x.IsDefined(typeof(Core.TypesProviderAttribute)))
                .ToArray();
        }

        public static readonly Assembly[] Assemblies;
    }
}
