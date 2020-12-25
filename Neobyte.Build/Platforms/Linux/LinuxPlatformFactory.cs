using Neobyte.Build.Framework;
using Neobyte.Build.Toolchains;
using Neobyte.Build.Toolchains.Clang;
using System;

namespace Neobyte.Build.Platforms.Linux
{
    sealed class LinuxPlatformFactory
        : PlatformFactory
    {
        public LinuxPlatformFactory(TargetArchitecture architecture, TargetToolchain toolchain)
            : base(TargetPlatform.Linux, architecture, toolchain)
        {

        }

        public override TargetContext CreateContext(Profile profile)
        {
            var settings = profile.GetSection<LinuxPlatformSettings>();

            return new TargetContext(
                this.CreatePlatform(settings, profile),
                this.CreateToolchain(settings, profile));
        }

        private PlatformBase CreatePlatform(LinuxPlatformSettings settings, Profile profile)
        {
            return new LinuxPlatform(profile, this.Architecture, settings);
        }

        private ToolchainBase CreateToolchain(LinuxPlatformSettings settings, Profile profile)
        {
            if (this.Toolchain == TargetToolchain.Clang)
            {
                return new ClangToolchain(
                    profile,
                    this.Platform,
                    this.Architecture,
                    settings.Clang);
            }

            throw new NotSupportedException();
        }
    }
}
