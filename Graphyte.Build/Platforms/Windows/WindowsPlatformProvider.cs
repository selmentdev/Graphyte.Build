using Graphyte.Build.Toolchains.VisualStudio;
using System;
using System.Collections.Generic;
using System.Text;

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
            return new MsvcToolchain(profile, this.ArchitectureType);
        }
    }

    sealed class WindowsPlatformProvider
        : IPlatformsProvider
    {
        public IEnumerable<BasePlatformFactory> Provide()
        {
            yield return new WindowsPlatformFactory(ArchitectureType.X64, ToolchainType.MSVC);
            yield return new WindowsPlatformFactory(ArchitectureType.X86, ToolchainType.MSVC);
            yield return new WindowsPlatformFactory(ArchitectureType.ARM, ToolchainType.MSVC);
            yield return new WindowsPlatformFactory(ArchitectureType.ARM64, ToolchainType.MSVC);

            yield return new WindowsPlatformFactory(ArchitectureType.X64, ToolchainType.Clang);
            yield return new WindowsPlatformFactory(ArchitectureType.X86, ToolchainType.Clang);
            yield return new WindowsPlatformFactory(ArchitectureType.ARM, ToolchainType.Clang);
            yield return new WindowsPlatformFactory(ArchitectureType.ARM64, ToolchainType.Clang);

            yield return new WindowsPlatformFactory(ArchitectureType.X64, ToolchainType.ClangCL);
            yield return new WindowsPlatformFactory(ArchitectureType.X86, ToolchainType.ClangCL);
            yield return new WindowsPlatformFactory(ArchitectureType.ARM, ToolchainType.ClangCL);
            yield return new WindowsPlatformFactory(ArchitectureType.ARM64, ToolchainType.ClangCL);
        }
    }
}
