using System;
using System.Linq;

namespace Graphyte.Build.Platforms
{
    public class PlatformProvider
    {
        private static BasePlatform[] Discover()
        {
            var baseType = typeof(BasePlatform);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(baseType) && !x.IsAbstract && x.IsClass)
                .Select(x => (BasePlatform)Activator.CreateInstance(x))
                .ToArray();
        }

        private readonly BasePlatform[] m_Platforms;

        public PlatformProvider()
        {
            this.m_Platforms = PlatformProvider.Discover();
        }

        public BasePlatform GetPlatform(TargetTuple tuple)
        {
            return this.GetPlatforms(tuple).FirstOrDefault();
        }

        public BasePlatform[] GetPlatforms(TargetTuple tuple)
        {
            return this.m_Platforms.Where(x => x.IsSupported(tuple)).ToArray();
        }

        public BasePlatform[] GetPlatforms()
        {
            return this.m_Platforms;
        }
    }
}
