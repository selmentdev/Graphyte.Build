using Graphyte.Build.Framework;
using System.Collections.Generic;

namespace Graphyte.Build.Platforms.Android
{
    [PlatformFactoryProvider]
    public sealed class AndroidPlatformFactoryProvider : PlatformFactoryProvider
    {
        public override IEnumerable<PlatformFactory> Provide()
        {
            yield return new AndroidPlatformFactory(TargetArchitecture.Arm64, TargetToolchain.Clang);
        }
    }
}
