using Graphyte.Build.Framework;
using Graphyte.Build.Toolchains;
using Graphyte.Build.Toolchains.Clang;
using System;

namespace Graphyte.Build.Platforms.Linux
{
    sealed class LinuxPlatformFactory
        : PlatformFactory
    {
        public LinuxPlatformFactory(TargetArchitecture architecture, TargetToolchain toolchain)
            : base(TargetPlatform.Linux, architecture, toolchain)
        {

        }

        public override PlatformBase CreatePlatform(Profile profile)
        {
            var settings = profile.GetSection<LinuxPlatformSettings>();

            return new LinuxPlatform(profile, this.Architecture, settings);
        }

        public override ToolchainBase CreateToolchain(Profile profile)
        {
            var settings = profile.GetSection<LinuxPlatformSettings>();

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
