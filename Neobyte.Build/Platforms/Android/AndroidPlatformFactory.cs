using Neobyte.Build.Framework;
using Neobyte.Build.Toolchains;
using Neobyte.Build.Toolchains.Clang;

namespace Neobyte.Build.Platforms.Android
{
    sealed class AndroidPlatformFactory
        : PlatformFactory
    {
        public AndroidPlatformFactory(TargetArchitecture architecture, TargetToolchain toolchain)
            : base(TargetPlatform.Android, architecture, toolchain)
        { }

        public override PlatformBase CreatePlatform(Profile profile)
        {
            var settings = profile.GetSection<AndroidPlatformSettings>();

            return new AndroidPlatform(profile, this.Architecture, settings);
        }

        public override ToolchainBase CreateToolchain(Profile profile)
        {
            var settings = profile.GetSection<AndroidPlatformSettings>();

            return new ClangToolchain(profile, this.Platform, this.Architecture, settings.Clang);
        }
    }
}

