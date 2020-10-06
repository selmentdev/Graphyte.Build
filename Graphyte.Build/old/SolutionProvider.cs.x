using System;
using System.Linq;

namespace Graphyte.Build
{
    public sealed class SolutionProvider
    {
        private static Solution[] Discover()
        {
            var baseType = typeof(Solution);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(baseType) && x.IsClass && !x.IsAbstract)
                .Select(x => (Solution)Activator.CreateInstance(x))
                .ToArray();
        }

        private readonly Solution[] m_Solutions;

        public SolutionProvider()
        {
            this.m_Solutions = SolutionProvider.Discover();
        }

        public Solution[] GetSolutions()
        {
            return this.m_Solutions;
        }
    }
}
