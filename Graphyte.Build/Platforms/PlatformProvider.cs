using System;
using System.Linq;

namespace Graphyte.Build.Platforms
{
    public sealed class PlatformProvider
    {
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

        public PlatformProvider()
        {
            this.m_Platforms = PlatformProvider.Discover();
        }

        public BasePlatform GetPlatform(TargetTuple targetTuple)
        {
            return this.GetPlatforms(targetTuple).FirstOrDefault();
        }

        public BasePlatform[] GetPlatforms(TargetTuple targetTuple)
        {
            return this.m_Platforms.Where(x => x.IsSupported(targetTuple)).ToArray();
        }

        public BasePlatform[] GetPLatforms()
        {
            return this.m_Platforms;
        }
    }
}
