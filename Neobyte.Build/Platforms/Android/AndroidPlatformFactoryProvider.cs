using Neobyte.Build.Framework;
using System.Collections.Generic;

namespace Neobyte.Build.Platforms.Android
{
    [PlatformFactoryProvider]
    public sealed class AndroidPlatformFactoryProvider
        : PlatformFactoryProvider
    {
        public override IEnumerable<PlatformFactory> Provide()
        {
            yield return new AndroidPlatformFactory(TargetArchitecture.Arm64, TargetToolchain.Clang);
        }
    }
}
