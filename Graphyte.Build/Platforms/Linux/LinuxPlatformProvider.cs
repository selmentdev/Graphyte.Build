using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build.Platforms.Linux
{
    sealed class LinuxPlatformFactory : BasePlatformFactory
    {
        public LinuxPlatformFactory(
            ArchitectureType architectureType,
            ToolchainType toolchainType)
            : base(
                  PlatformType.Linux,
                  architectureType,
                  toolchainType)
        {
        }

        public override BasePlatform CreatePlatform(Profile profile)
        {
            return new LinuxPlatform(profile, this.ArchitectureType);
        }

        public override BaseToolchain CreateToolchain(Profile profile)
        {
            throw new NotImplementedException();
        }
    }

    sealed class LinuxPlatformProvider
        : IPlatformProvider
    {
        public IEnumerable<BasePlatformFactory> Provide()
        {
            yield return new LinuxPlatformFactory(ArchitectureType.ARM64, ToolchainType.Clang);
            yield return new LinuxPlatformFactory(ArchitectureType.X64, ToolchainType.Clang);
            yield return new LinuxPlatformFactory(ArchitectureType.ARM, ToolchainType.Clang);
            yield return new LinuxPlatformFactory(ArchitectureType.X86, ToolchainType.Clang);
        }
    }
}
