using Neobyte.Build.Framework;
using Neobyte.Build.Toolchains;
using Neobyte.Build.Toolchains.VisualStudio;

namespace Neobyte.Build.Platforms.Windows
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
