using Graphyte.Build.Framework;
using Graphyte.Build.Toolchains;
using Graphyte.Build.Toolchains.Clang;
using Graphyte.Build.Toolchains.VisualStudio;
using System;

namespace Graphyte.Build.Platforms.Windows
{
    sealed class WindowsPlatformFactory : PlatformFactory
    {
        public WindowsPlatformFactory(TargetArchitecture targetArchitecture, TargetToolchain targetToolchain)
            : base(TargetPlatform.Windows, targetArchitecture, targetToolchain)
        {
        }

        public override Platform CreatePlatform(Profile profile)
        {
            var settings = profile.GetSection<WindowsPlatformSettings>();

            return new WindowsPlatform(profile, this.TargetArchitecture, settings);
        }

        public override Toolchain CreateToolchain(Profile profile)
        {
            var settings = profile.GetSection<WindowsPlatformSettings>();

            if (this.TargetToolchain == TargetToolchain.MSVC)
            {
                return new VisualStudioToolchain(
                    profile,
                    this.TargetArchitecture,
                    settings.VisualStudio);
            }
            else if (this.TargetToolchain == TargetToolchain.Clang)
            {
                return new ClangToolchain(
                    profile,
                    this.TargetPlatform,
                    this.TargetArchitecture,
                    settings.Clang);
            }
            else if (this.TargetToolchain == TargetToolchain.ClangCL)
            {
                return new ClangCLToolchain(
                    profile,
                    this.TargetPlatform,
                    this.TargetArchitecture,
                    settings.ClangCL);
            }

            throw new NotSupportedException("Toolchain not supported");
        }
    }
}
