using System.Collections.Generic;

namespace Graphyte.Build.Platforms
{
    public abstract class PlatformFactoryProvider
    {
        public abstract IEnumerable<PlatformFactory> Provide();
    }
}
