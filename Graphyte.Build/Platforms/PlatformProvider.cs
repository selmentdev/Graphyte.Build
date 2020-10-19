using System;
using System.Linq;

namespace Graphyte.Build.Platforms
{
    /// <summary>
    /// Provides all discovered platforms in current AppDomain.
    /// </summary>
    public sealed class PlatformProvider
    {
        /// <summary>
        /// Discovers all platforms in current AppDomain.
        /// </summary>
        /// <returns>The array of discovered platforms.</returns>
        private static BasePlatform[] Discover()
        {
            var platformType = typeof(BasePlatform);

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(platformType) && x.IsClass && !x.IsAbstract && x.IsVisible && x.IsSealed)
                .Select(x => (BasePlatform)Activator.CreateInstance(x))
                .ToArray();
        }

        private readonly BasePlatform[] m_Platforms;

        /// <summary>
        /// Creates new instance of platform provider.
        /// </summary>
        public PlatformProvider()
        {
            this.m_Platforms = PlatformProvider.Discover();
        }

        /// <summary>
        /// Gets all platforms supporting specified target tuple.
        /// </summary>
        /// <param name="targetTuple">A target tuple.</param>
        /// <returns>The array of platforms supporting given target tuple.</returns>
        public BasePlatform[] GetPlatforms(TargetTuple targetTuple)
        {
            return this.m_Platforms.Where(x => x.IsTargetTupleSupported(targetTuple)).ToArray();
        }

        /// <summary>
        /// Gets all discovered platforms.
        /// </summary>
        /// <returns>The array of all discovered platforms.</returns>
        public BasePlatform[] GetPlatforms()
        {
            return this.m_Platforms;
        }
    }
}
