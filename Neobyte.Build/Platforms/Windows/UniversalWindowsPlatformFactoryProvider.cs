using Neobyte.Build.Framework;
using System.Collections.Generic;

namespace Neobyte.Build.Platforms.Windows
{
    [PlatformFactoryProvider]
    public sealed class UniversalWindowsPlatformFactoryProvider
        : PlatformFactoryProvider
    {
        public override IEnumerable<PlatformFactory> Provide()
        {
            yield return new UniversalWindowsPlatformFactory(TargetArchitecture.X64, TargetToolchain.MSVC);
            yield return new UniversalWindowsPlatformFactory(TargetArchitecture.Arm64, TargetToolchain.MSVC);
        }
    }
}
