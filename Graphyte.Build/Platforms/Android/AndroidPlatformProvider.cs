using System.Collections.Generic;

namespace Graphyte.Build.Platforms.Android
{
    sealed class AndroidPlatformFactory
        : BasePlatformFactory
    {
        public AndroidPlatformFactory(
            ArchitectureType architectureType,
            ToolchainType toolchainType)
            : base(
                  PlatformType.Android,
                  architectureType,
                  toolchainType)
        {
        }

        public override BasePlatform CreatePlatform(Profile profile)
        {
            return new AndroidPlatform(profile, this.ArchitectureType);
        }

        public override BaseToolchain CreateToolchain(Profile profile)
        {
            throw new System.NotImplementedException();
        }
    }

    sealed class AndroidPlatformProvider
        : IPlatformsProvider
    {
        public IEnumerable<BasePlatformFactory> Provide()
        {
            yield return new AndroidPlatformFactory(ArchitectureType.ARM64, ToolchainType.Clang);
        }
    }
}
