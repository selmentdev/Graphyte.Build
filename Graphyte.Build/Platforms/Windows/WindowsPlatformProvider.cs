using Graphyte.Build.Framework;
using System.Collections.Generic;

namespace Graphyte.Build.Platforms.Windows
{
    [PlatformFactoryProvider]
    public sealed class WindowsPlatformProvider : PlatformFactoryProvider
    {
        public override IEnumerable<PlatformFactory> Provide()
        {
            yield return new WindowsPlatformFactory(TargetArchitecture.X64, TargetToolchain.MSVC);
            yield return new WindowsPlatformFactory(TargetArchitecture.Arm64, TargetToolchain.MSVC);
            yield return new WindowsPlatformFactory(TargetArchitecture.X64, TargetToolchain.Clang);
            yield return new WindowsPlatformFactory(TargetArchitecture.X64, TargetToolchain.ClangCL);
        }
    }
}
