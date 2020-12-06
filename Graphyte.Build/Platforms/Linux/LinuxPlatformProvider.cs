using System;
using System.Collections.Generic;
using System.Text;
using Graphyte.Build.Toolchains.Clang;

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
            if (this.ToolchainType == ToolchainType.Clang)
            {
                return new ClangToolchain(
                    profile,
                    this.ArchitectureType,
                    PlatformType.Linux);
            }

            throw new NotSupportedException();
        }
    }

    sealed class LinuxPlatformProvider
        : IPlatformsProvider
    {
        public IEnumerable<BasePlatformFactory> Provide()
        {
            yield return new LinuxPlatformFactory(ArchitectureType.ARM64, ToolchainType.Clang);
            yield return new LinuxPlatformFactory(ArchitectureType.X64, ToolchainType.Clang);
        }
    }
}
