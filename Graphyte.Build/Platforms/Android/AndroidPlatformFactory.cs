using Graphyte.Build.Framework;
using Graphyte.Build.Toolchains;
using Graphyte.Build.Toolchains.Clang;

namespace Graphyte.Build.Platforms.Android
{
    sealed class AndroidPlatformFactory : PlatformFactory
    {
        public AndroidPlatformFactory(TargetArchitecture targetArchitecture, TargetToolchain targetToolchain)
            :base(TargetPlatform.Android, targetArchitecture, targetToolchain)
            { }

        public override Platform CreatePlatform(Profile profile)
        {
            var settings = profile.GetSection<AndroidPlatformSettings>();

            return new AndroidPlatform(profile, this.TargetArchitecture, settings);
        }

        public override Toolchain CreateToolchain(Profile profile)
        {
            var settings = profile.GetSection<AndroidPlatformSettings>();

            return new ClangToolchain(profile, this.TargetPlatform, this.TargetArchitecture, settings.Clang);
        }
    }
}

