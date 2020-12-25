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

        public override TargetContext CreateContext(Profile profile)
        {
            var settings = profile.GetSection<UniversalWindowsPlatformSettings>();

            return new TargetContext(
                this.CreatePlatform(settings, profile),
                this.CreateToolchain(settings, profile));
        }

        private PlatformBase CreatePlatform(UniversalWindowsPlatformSettings settings, Profile profile)
        {
            return new UniversalWindowsPlatform(profile, this.Architecture, settings);
        }

        private ToolchainBase CreateToolchain(UniversalWindowsPlatformSettings settings, Profile profile)
        {
            return new VisualStudioToolchain(profile, this.Architecture, settings.VisualStudio);
        }
    }
}
