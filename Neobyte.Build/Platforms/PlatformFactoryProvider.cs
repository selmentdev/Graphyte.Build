using System.Collections.Generic;

namespace Neobyte.Build.Platforms
{
    public abstract class PlatformFactoryProvider
    {
        public abstract IEnumerable<PlatformFactory> Provide();
    }
}
