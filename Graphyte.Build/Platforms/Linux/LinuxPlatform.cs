using System;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Linux
{
    public sealed class LinuxPlatformSettings
        : BasePlatformSettings
    {
    }

    sealed class LinuxPlatform
        : BasePlatform
    {
        public LinuxPlatform(
            Profile profile,
            ArchitectureType architectureType)
            : base(
                  profile,
                  architectureType)
        {
            this.m_Settings = profile.GetSection<LinuxPlatformSettings>();

            // Default include paths
            this.IncludePaths = new string[] { };

            // Default library paths
            this.LibraryPaths = new string[] { };
        }

        public override PlatformType PlatformType => PlatformType.Linux;

        public override bool IsPlatformKind(PlatformKind platformKind)
        {
            switch (platformKind)
            {
                case PlatformKind.Desktop:
                case PlatformKind.Server:
                    return true;
                case PlatformKind.Mobile:
                case PlatformKind.Console:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(platformKind));
        }

        public override void PreConfigureTarget(Target target)
        {
        }

        public override void PostConfigureTarget(Target target)
        {
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
                    return name;
                case TargetType.Application:
                    return $@"{name}.elf";
                case TargetType.Default:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(targetType));
        }

        private readonly LinuxPlatformSettings m_Settings;
    }
}
