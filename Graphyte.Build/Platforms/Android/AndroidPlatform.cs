using System;

namespace Graphyte.Build.Platforms.Android
{
    public sealed class AndroidPlatformSettings
        : BasePlatformSettings
    {
        /// <summary>
        /// Provides path to Android SDK.
        /// </summary>
        public string SdkPath { get; set; }

        /// <summary>
        /// Provides path to Anrdoid NDK.
        /// </summary>
        public string NdkPath { get; set; }

        public int TargetApiLevel { get; set; }
    }

    sealed class AndroidPlatform
        : BasePlatform
    {
        public AndroidPlatform(
            Profile profile,
            ArchitectureType architectureType)
            : base(
                  profile,
                  architectureType)
        {
            this.m_Settings = profile.GetSection<AndroidPlatformSettings>();
        }

        public override PlatformType PlatformType => PlatformType.Android;

        public override bool IsPlatformKind(PlatformKind platformKind)
        {
            switch (platformKind)
            {
                case PlatformKind.Mobile:
                    return true;
                case PlatformKind.Desktop:
                case PlatformKind.Console:
                case PlatformKind.Server:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(platformKind));
        }

        public override string AdjustTargetName(string name, TargetType targetType)
        {
            switch (targetType)
            {
                case TargetType.SharedLibrary:
                    return $@"lib{name}.so";
                case TargetType.StaticLibrary:
                    return $@"lib{name}.a";
                case TargetType.HeaderLibrary:
                case TargetType.Application:
                    return name;
                case TargetType.Default:
                    break;
            };

            throw new ArgumentOutOfRangeException(nameof(targetType));
        }

        private readonly AndroidPlatformSettings m_Settings;
    }
}
