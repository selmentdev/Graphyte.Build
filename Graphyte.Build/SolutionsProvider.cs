using System;
using System.Linq;

namespace Graphyte.Build
{
    public sealed class SolutionsProvider
    {
        private static Type[] Discover()
        {
            var baseType = typeof(Solution);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(baseType) && x.IsClass && !x.IsAbstract && x.IsVisible)
                .ToArray();
        }

        public Type[] Solutions { get; } = SolutionsProvider.Discover();

        public Solution[] Create()
        {
            return this.Solutions.Select(x => Activator.CreateInstance(x)).Cast<Solution>().ToArray();
        }
    }
}
