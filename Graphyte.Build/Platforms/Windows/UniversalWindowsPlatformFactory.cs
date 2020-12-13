using Graphyte.Build.Framework;
using Graphyte.Build.Toolchains;
using Graphyte.Build.Toolchains.VisualStudio;

namespace Graphyte.Build.Platforms.Windows
{
    sealed class UniversalWindowsPlatformFactory
         : PlatformFactory
    {
        public UniversalWindowsPlatformFactory(TargetArchitecture targetArchitecture, TargetToolchain targetToolchain)
            : base(TargetPlatform.UniversalWindows, targetArchitecture, targetToolchain)
        {
        }

        public override Platform CreatePlatform(Profile profile)
        {
            var settings = profile.GetSection<UniversalWindowsPlatformSettings>();

            return new UniversalWindowsPlatform(profile, this.TargetArchitecture, settings);
        }

        public override Toolchain CreateToolchain(Profile profile)
        {
            var settings = profile.GetSection<UniversalWindowsPlatformSettings>();

            return new VisualStudioToolchain(profile, this.TargetArchitecture, settings.VisualStudio);
        }
    }
}
