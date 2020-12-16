using Neobyte.Build.Framework;
using System.Collections.Generic;

namespace Neobyte.Build.Platforms.Linux
{
    [PlatformFactoryProvider]
    public sealed class LinuxPlatformFactoryProvider
        : PlatformFactoryProvider
    {
        public override IEnumerable<PlatformFactory> Provide()
        {
            yield return new LinuxPlatformFactory(TargetArchitecture.X64, TargetToolchain.Clang);
            yield return new LinuxPlatformFactory(TargetArchitecture.Arm64, TargetToolchain.Clang);
        }
    }
}
