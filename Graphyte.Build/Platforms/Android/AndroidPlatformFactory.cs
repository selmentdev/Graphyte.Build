using Graphyte.Build.Framework;
using Graphyte.Build.Toolchains;
using Graphyte.Build.Toolchains.Clang;

namespace Graphyte.Build.Platforms.Android
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

