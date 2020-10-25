using Graphyte.Build.Toolchains;
using System;
using System.Diagnostics;
using System.Linq;

namespace Graphyte.Build.Platforms
{
    /// <summary>
    /// Provides all discovered platforms in current AppDomain.
    /// </summary>
    public sealed class PlatformsProvider
    {
        /// <summary>
        /// Discovers all platforms in current AppDomain.
        /// </summary>
        /// <returns>The array of discovered platforms.</returns>
        private static Type[] Discover()
        {
            var baseType = typeof(BasePlatform);

            var result = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(baseType) && x.IsClass && !x.IsAbstract && x.IsVisible && x.IsSealed)
                .ToArray();

#if DEBUG
            foreach (var platform in result)
            {
                Debug.Assert(platform.Name.EndsWith("Platform"));
            }
#endif

            return result;
        }

        public Type[] Platforms { get; } = PlatformsProvider.Discover();

        public BasePlatform Create(PlatformType platform)
        {
            var name = $@"{platform}Platform";
            var type = this.Platforms.FirstOrDefault(x => x.Name == name);

            if (type != null)
            {
                return (BasePlatform)Activator.CreateInstance(type);
            }

            return null;
        }
    }
}
