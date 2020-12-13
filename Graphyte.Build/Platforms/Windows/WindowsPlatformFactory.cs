using Graphyte.Build.Framework;
using Graphyte.Build.Toolchains;
using Graphyte.Build.Toolchains.Clang;
using Graphyte.Build.Toolchains.VisualStudio;
using System;

namespace Graphyte.Build.Platforms.Windows
{
    sealed class WindowsPlatformFactory
        : PlatformFactory
    {
        public WindowsPlatformFactory(TargetArchitecture architecture, TargetToolchain toolchain)
            : base(TargetPlatform.Windows, architecture, toolchain)
        {
        }

        public override PlatformBase CreatePlatform(Profile profile)
        {
            var settings = profile.GetSection<WindowsPlatformSettings>();

            return new WindowsPlatform(profile, this.Architecture, settings);
        }

        public override ToolchainBase CreateToolchain(Profile profile)
        {
            var settings = profile.GetSection<WindowsPlatformSettings>();

            if (this.Toolchain == TargetToolchain.MSVC)
            {
                return new VisualStudioToolchain(
                    profile,
                    this.Architecture,
                    settings.VisualStudio);
            }
            else if (this.Toolchain == TargetToolchain.Clang)
            {
                return new ClangToolchain(
                    profile,
                    this.Platform,
                    this.Architecture,
                    settings.Clang);
            }
            else if (this.Toolchain == TargetToolchain.ClangCL)
            {
                return new ClangCLToolchain(
                    profile,
                    this.Platform,
                    this.Architecture,
                    settings.ClangCL);
            }

            throw new NotSupportedException("Toolchain not supported");
        }
    }
}
