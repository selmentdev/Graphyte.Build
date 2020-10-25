using System;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Linux
{
    public sealed class LinuxPlatform
        : BasePlatform
    {
        public override bool IsHostSupported
            => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        private static readonly ArchitectureType[] g_SupportedArchitectures = new[]
        {
            ArchitectureType.ARM64,
            ArchitectureType.X64,
        };

        public override ArchitectureType[] Architectures => LinuxPlatform.g_SupportedArchitectures;

        public override PlatformType Type => PlatformType.Linux;

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

        private LinuxPlatformSettings m_Settings;

        public override void Initialize(Profile profile)
        {
            this.m_Settings = profile.GetSection<LinuxPlatformSettings>();
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
    }
}
