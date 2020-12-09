using Graphyte.Build.Toolchains.Clang;
using Graphyte.Build.Toolchains.ClangCL;
using Graphyte.Build.Toolchains.VisualStudio;
using System;
using System.Collections.Generic;

namespace Graphyte.Build.Platforms.Windows
{
    sealed class WindowsPlatformFactory
        : BasePlatformFactory
    {
        public WindowsPlatformFactory(
            ArchitectureType architectureType,
            ToolchainType toolchainType)
            : base(
                  PlatformType.Windows,
                  architectureType,
                  toolchainType)
        {
        }

        public override BasePlatform CreatePlatform(Profile profile)
        {
            return new WindowsPlatform(profile, this.ArchitectureType);
        }

        public override BaseToolchain CreateToolchain(Profile profile)
        {
            if (this.ToolchainType == ToolchainType.MSVC)
            {
                return new MsvcToolchain(profile, this.ArchitectureType);
            }
            else if (this.ToolchainType == ToolchainType.Clang)
            {
                return new ClangToolchain(profile, this.ArchitectureType, this.PlatformType);
            }
            else if (this.ToolchainType == ToolchainType.ClangCL)
            {
                return new ClangCLToolchain(profile, this.ArchitectureType);
            }

            throw new NotSupportedException("Toolchain not supported");
        }
    }

    sealed class WindowsPlatformProvider
        : IPlatformsProvider
    {
        public IEnumerable<BasePlatformFactory> Provide()
        {
            yield return new WindowsPlatformFactory(ArchitectureType.X64, ToolchainType.MSVC);
            yield return new WindowsPlatformFactory(ArchitectureType.ARM64, ToolchainType.MSVC);

            yield return new WindowsPlatformFactory(ArchitectureType.X64, ToolchainType.Clang);

            yield return new WindowsPlatformFactory(ArchitectureType.X64, ToolchainType.ClangCL);
        }
    }
}
