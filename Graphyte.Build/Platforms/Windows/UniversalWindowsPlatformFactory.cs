using Graphyte.Build.Framework;
using Graphyte.Build.Toolchains;
using Graphyte.Build.Toolchains.VisualStudio;

namespace Graphyte.Build.Platforms.Windows
{
    sealed class UniversalWindowsPlatformFactory
        : PlatformFactory
    {
        public UniversalWindowsPlatformFactory(TargetArchitecture architecture, TargetToolchain toolchain)
            : base(TargetPlatform.UniversalWindows, architecture, toolchain)
        {
        }

        public override PlatformBase CreatePlatform(Profile profile)
        {
            var settings = profile.GetSection<UniversalWindowsPlatformSettings>();

            return new UniversalWindowsPlatform(profile, this.Architecture, settings);
        }

        public override ToolchainBase CreateToolchain(Profile profile)
        {
            var settings = profile.GetSection<UniversalWindowsPlatformSettings>();

            return new VisualStudioToolchain(profile, this.Architecture, settings.VisualStudio);
        }
    }
}
