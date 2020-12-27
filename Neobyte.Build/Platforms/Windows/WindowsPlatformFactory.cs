using Neobyte.Build.Framework;
using Neobyte.Build.Toolchains;
using Neobyte.Build.Toolchains.Clang;
using Neobyte.Build.Toolchains.VisualStudio;
using System;

namespace Neobyte.Build.Platforms.Windows
{
    sealed class WindowsPlatformFactory
        : PlatformFactory
    {
        public WindowsPlatformFactory(TargetArchitecture architecture, TargetToolchain toolchain)
            : base(TargetPlatform.Windows, architecture, toolchain)
        {
        }

        public override TargetContext CreateContext(Profile profile)
        {
            var settings = profile.GetSection<WindowsPlatformSettings>()!;

            return new TargetContext(
                this.CreatePlatform(settings, profile),
                this.CreateToolchain(settings, profile));
        }

        private PlatformBase CreatePlatform(WindowsPlatformSettings settings, Profile profile)
        {
            return new WindowsPlatform(profile, this.Architecture, settings);
        }

        private ToolchainBase CreateToolchain(WindowsPlatformSettings settings, Profile profile)
        {
            if (this.Toolchain == TargetToolchain.MSVC)
            {
                return new VisualStudioToolchain(
                    profile,
                    this.Architecture,
                    settings.VisualStudio!);
            }
            else if (this.Toolchain == TargetToolchain.Clang)
            {
                return new ClangToolchain(
                    profile,
                    this.Platform,
                    this.Architecture,
                    settings.Clang!);
            }
            else if (this.Toolchain == TargetToolchain.ClangCL)
            {
                return new ClangCLToolchain(
                    profile,
                    this.Platform,
                    this.Architecture,
                    settings.ClangCL!);
            }

            throw new NotSupportedException("Toolchain not supported");
        }
    }
}
