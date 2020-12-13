using Graphyte.Build.Framework;
using Graphyte.Build.Toolchains;
using Graphyte.Build.Toolchains.Clang;
using System;

namespace Graphyte.Build.Platforms.Linux
{
    sealed class LinuxPlatformFactory : PlatformFactory
    {
        public LinuxPlatformFactory(
            TargetArchitecture targetArchitecture,
            TargetToolchain targetToolchain)
            : base(TargetPlatform.Linux, targetArchitecture, targetToolchain)
        {

        }

        public override Platform CreatePlatform(Profile profile)
        {
            var settings = profile.GetSection<LinuxPlatformSettings>();

            return new LinuxPlatform(profile, this.TargetArchitecture, settings);
        }

        public override Toolchain CreateToolchain(Profile profile)
        {
            var settings = profile.GetSection<LinuxPlatformSettings>();

            if (this.TargetToolchain == TargetToolchain.Clang)
            {
                return new ClangToolchain(
                    profile,
                    this.TargetPlatform,
                    this.TargetArchitecture,
                    settings.Clang);
            }

            throw new NotSupportedException();
        }
    }
}
