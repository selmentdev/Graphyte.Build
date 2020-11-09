using Graphyte.Build.Toolchains.VisualStudio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build.Platforms.Windows
{
    sealed class UniversalWindowsPlatformFactory
        : BasePlatformFactory
    {
        public UniversalWindowsPlatformFactory(
            ArchitectureType architectureType,
            ToolchainType toolchainType)
            : base(
                  PlatformType.UniversalWindows,
                  architectureType,
                  toolchainType)
        {
        }

        public override BasePlatform CreatePlatform(Profile profile)
        {
            return new UniversalWindowsPlatform(profile, this.ArchitectureType);
        }

        public override BaseToolchain CreateToolchain(Profile profile)
        {
            return new MsvcToolchain(profile, this.ArchitectureType);
        }
    }

    sealed class UniversalWindowsPlatformProvider
        : IPlatformsProvider
    {
        public IEnumerable<BasePlatformFactory> Provide()
        {
            yield return new UniversalWindowsPlatformFactory(ArchitectureType.X64, ToolchainType.MSVC);
            yield return new UniversalWindowsPlatformFactory(ArchitectureType.X86, ToolchainType.MSVC);
            yield return new UniversalWindowsPlatformFactory(ArchitectureType.ARM, ToolchainType.MSVC);
            yield return new UniversalWindowsPlatformFactory(ArchitectureType.ARM64, ToolchainType.MSVC);
        }
    }
}
