using Neobyte.Build.Framework;
using Neobyte.Build.Toolchains;
using Neobyte.Build.Toolchains.Clang;
using System;

namespace Neobyte.Build.Platforms.Android
{
    sealed class AndroidPlatformFactory
        : PlatformFactory
    {
        public AndroidPlatformFactory(TargetArchitecture architecture, TargetToolchain toolchain)
            : base(TargetPlatform.Android, architecture, toolchain)
        { }

        public override TargetContext CreateContext(Profile profile)
        {
            var settings = profile.GetSection<AndroidPlatformSettings>();

            if (settings == null)
            {
                throw new NotSupportedException();
            }

            return new TargetContext(
                this.CreatePlatform(settings, profile),
                this.CreateToolchain(settings, profile));
        }

        private PlatformBase CreatePlatform(AndroidPlatformSettings settings, Profile profile)
        {

            return new AndroidPlatform(profile, this.Architecture, settings);
        }

        private ToolchainBase CreateToolchain(AndroidPlatformSettings settings, Profile profile)
        {
            return new ClangToolchain(profile, this.Platform, this.Architecture, settings.Clang!);
        }
    }
}

