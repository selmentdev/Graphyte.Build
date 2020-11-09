using System;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Windows
{
    public sealed class UniversalWindowsPlatformSettings
        : BasePlatformSettings
    {
        public string WindowsSdkVersion { get; set; }
    }

    sealed class UniversalWindowsPlatform
        : BaseWindowsPlatform
    {
        public UniversalWindowsPlatform(
            Profile profile,
            ArchitectureType architectureType)
            : base(
                  profile,
                  architectureType)
        {
            this.m_Settings = profile.GetSection<UniversalWindowsPlatformSettings>();
            var version = this.m_Settings.WindowsSdkVersion;

            this.InitializeBasePaths(version);
        }

        public override PlatformType PlatformType => PlatformType.UniversalWindows;

        public override bool IsPlatformKind(PlatformKind platformKind)
        {
            switch (platformKind)
            {
                case PlatformKind.Desktop:
                case PlatformKind.Mobile:
                    return true;
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
                    return $@"lib{name}.dll";
                case TargetType.StaticLibrary:
                    return $@"lib{name}.lib";
                case TargetType.HeaderLibrary:
                    return name;
                case TargetType.Application:
                    return $@"{name}.exe";
                case TargetType.Default:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(targetType));
        }

        private readonly UniversalWindowsPlatformSettings m_Settings;
    }
}
